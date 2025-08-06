using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.ViewModels.Products;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;



        public ProductsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // async olmasına rağmen await kullanmadık neden?
            return Ok(_productReadRepository.GetAll(false));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id, false));
        }


        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {


            await _productWriteRepository.AddAsync(new()
            {   
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock

            });
            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created); // 201 Created
        }


        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            //defoult tracing true ama ben yinede yazdım.
            Product product = await _productReadRepository.GetByIdAsync(model.Id,true);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            _productWriteRepository.Update(product);
            await _productWriteRepository.SaveAsync();
            return NoContent(); // 204 No Content
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return NoContent(); // ✅ 204 No Content
        }




    }
}
