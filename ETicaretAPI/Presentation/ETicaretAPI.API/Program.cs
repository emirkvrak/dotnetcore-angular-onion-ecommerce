using ETicaretAPI.Application.Validators.Products;
using ETicaretAPI.Infrastructure;
using ETicaretAPI.Infrastructure.Filters;
using ETicaretAPI.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureServices();



builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>

    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
)); // CORS ayarlarını ekleme









// ✅ FluentValidation: Güncel yaklaşım
builder.Services.AddFluentValidationAutoValidation(); // FluentValidation çalışsın
builder.Services.AddFluentValidationClientsideAdapters(); // (Opsiyonel) Razor/Blazor destekler
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>(); // Validator'ları yükle

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); // Buraya eklendi
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true; // Otomatik model hatası dönüşünü engelle
});











// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ✅ Swagger için ekleme
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // ✅ Swagger UI aktifleştirildi
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseStaticFiles();


app.UseCors(); // CORS ayarlarını uygulamak için ekleme

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
