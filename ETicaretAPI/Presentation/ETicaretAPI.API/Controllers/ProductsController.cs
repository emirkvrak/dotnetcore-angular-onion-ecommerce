using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    // [ApiController] → Model binding/validation kolaylıkları,
    // otomatik 400 davranışı, [FromBody]/[FromQuery] varsayılanları vb. sağlar.
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Read/Write repository’ler DI ile gelir.
        // IWebHostEnvironment → wwwroot gibi fiziksel yollar için kullanılır (dosya upload).
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //Doysa için
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IFileReadRepository _fileReadRepository;

        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;

        private readonly IStorageService _storageService;

        public ProductsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment,
            IFileWriteRepository fileWriteRepository,
            IFileReadRepository fileReadRepository,
            IProductImageFileReadRepository productImageFileReadRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository,
            IInvoiceFileReadRepository invoiceFileReadRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository,
            IStorageService storageService)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _storageService = storageService;
        }

        // GET /api/products?Page=0&Size=5
        // [FromQuery] Pagination → QueryString’den Page/Size alınır (sayfalama parametreleri).
        // Neden async? DB’ye giden işlemleri asenkron yapmak, thread’leri bloklamamak için.
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Pagination pagination)
        {
            // Okuma amaçlı → tracking kapalı (performans).
            // Not: Sayfalama yaparken deterministik sonuçlar için her zaman OrderBy ekleyin.
            var query = _productReadRepository
                .GetAll(tracking: false)
                .OrderBy(p => p.CreatedDate); // veya Id/Name; sabit bir sıralama önemli

            // Toplam kayıt sayısı (COUNT) → async
            var totalCount = await query.CountAsync();

            // Sayfalama + projeksiyon → SQL tarafında çalışır (IQueryable).
            var products = await query
                .Skip(pagination.Page * pagination.Size)
                .Take(pagination.Size)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.CreatedDate,
                    p.UpdatedDate
                })
                .ToListAsync(); // DB’ye burada gidilir

            // 200 OK + payload
            return Ok(new { totalCount, products });
        }

        // GET /api/products/{id}
        // Neden async? DB’den tek kayıt çekiliyor (I/O).
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            // Okuma senaryosu → tracking=false performans için uygun.
            var product = await _productReadRepository.GetByIdAsync(id, tracking: false);
            if (product is null) return NotFound(); // 404
            return Ok(product); // 200
        }

        // POST /api/products
        // Body → VM_Create_Product (ViewModel). [ApiController] varsa invalid modelde 400 döner.
        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            // Yalnızca state işareti (Added). Asıl yazma SaveAsync’te.
            await _productWriteRepository.AddAsync(new Product
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });

            // Gerçek DB yazımı (INSERT) → async I/O
            await _productWriteRepository.SaveAsync();

            // 201 Created (resource location eklemek istersen CreatedAtAction kullanılabilir)
            return StatusCode((int)HttpStatusCode.Created);
        }

        // PUT /api/products
        // Güncelleme: önce mevcut kaydı al (tracking=true), alanları değiştir, Update ile Modified işaretle, SaveAsync ile yaz.
        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            // tracking=true: Change Tracker entity’yi takip etsin ki Update sonrası değişiklikler yazılsın.
            var product = await _productReadRepository.GetByIdAsync(model.Id, tracking: true);
            if (product is null) return NotFound();

            // Alanları güncelle (business/validation kontrolü burada yapılır).
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;

            // EF state → Modified (I/O yok; SaveAsync’te gidecek)
            _productWriteRepository.Update(product);

            // DB’ye UPDATE — async I/O
            await _productWriteRepository.SaveAsync();

            // 204 No Content: Başarılı güncellemede gövde yok.
            return NoContent();
        }

        // DELETE /api/products/{id}
        // Silme: id ile bul, Deleted olarak işaretle, SaveAsync ile yaz.
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            // Bul & işaretle (RemoveAsync bulma kısmı async, Remove state değiştirir)
            var removed = await _productWriteRepository.RemoveAsync(id);
            if (!removed) return NotFound(); // Id bulunamazsa 404 daha doğru

            // DB’ye DELETE — async I/O
            await _productWriteRepository.SaveAsync();

            // 200 OK (alternatif: 204 NoContent)
            return Ok();
        }

        // POST /api/products/upload
        // Çoklu dosya upload. İçerik tipini/form-data’yı MVC kendisi bağlar (Request.Form.Files).
        // Notlar:
        // - Güvenli dosya adı üret (GUID).
        // - İçerik tipi/uzantı whitelisti uygula.
        // - Büyük dosyada FileStream useAsync:true ve uygun buffer kullan.
        // - Dönen payload’ta kaydedilen dosyaların isimlerini vermek iyi bir DX sağlar.
        [HttpPost("[action]")]
        public async Task<ActionResult> Upload()
        {

            var datas = await _storageService.UploadAsync("files", Request.Form.Files);

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage = _storageService.StorageName

            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();


            //var datas = await _fileService.UploadAsync("resource/product-images", Request.Form.Files);
            //await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,

            //}).ToList());

            //await _productImageFileWriteRepository.SaveAsync();


            //var datas = await _fileService.UploadAsync("resource/invoice-images", Request.Form.Files);

            //await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //    Price = new Random().Next()
            //}).ToList());

            //await _invoiceFileWriteRepository.SaveAsync();


            //var datas = await _fileService.UploadAsync("resource/files-images", Request.Form.Files);
            //await _fileWriteRepository.AddRangeAsync(datas.Select(d => new ETicaretAPI.Domain.Entities.File()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,

            //}).ToList());

            //await _fileWriteRepository.SaveAsync();


            //var d1 = _fileReadRepository.GetAll(false);
            //var d2 = _invoiceFileReadRepository.GetAll(false);
            //var d3 = _productImageFileReadRepository.GetAll(false);

            return Ok();
            
        }
    }
}
