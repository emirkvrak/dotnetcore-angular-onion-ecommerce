using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ETicaretAPI.Infrastructure.Filters
{
    // ValidationFilter → ASP.NET Core pipeline'ında action'lar çalıştırılmadan önce
    // model doğrulama (ModelState) kontrolü yapmaya yarayan özel bir filter sınıfıdır.
    // IAsyncActionFilter → Asenkron action filter arayüzünü implemente eder.
    public class ValidationFilter : IAsyncActionFilter
    {
        // Action çalıştırılmadan ÖNCE veya SONRA devreye giren metot.
        // Parametreler:
        //  - context: Action çağrısı hakkındaki bilgiler (HttpContext, ModelState, RouteData vb.)
        //  - next: Pipeline'daki bir sonraki aşamayı çalıştırmak için kullanılan delegate
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // ModelState otomatik olarak DataAnnotations (ör. [Required], [MaxLength]) kontrollerini yapar.
            // Eğer model valid değilse bu if bloğuna girer.
            if (!context.ModelState.IsValid)
            {
                // Hatalı property'leri ve bunların hata mesajlarını yakalıyoruz.
                // Örn. "Name": ["Name alanı boş olamaz"] gibi.
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Any()) // Hata içeren property'leri seç
                    .ToDictionary(
                        kvp => kvp.Key, // property adı (örn. Name)
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray() // hata mesajları listesi
                    );

                // Hataları BadRequest (HTTP 400) olarak geri döndür.
                // context.Result → action çalıştırılmadan sonucu direkt olarak belirler.
                context.Result = new BadRequestObjectResult(errors);
                return; // Action metoduna hiç girilmez.
            }

            // Eğer ModelState valid ise, pipeline’daki bir sonraki aşamaya geçilir.
            // Bu, action metodunun gerçekten çalıştırılması anlamına gelir.
            await next();
        }
    }
}
