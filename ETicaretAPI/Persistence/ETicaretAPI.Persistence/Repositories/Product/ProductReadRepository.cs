using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories
{
    // ProductReadRepository:
    // Sadece Product tablosu için okuma (read) operasyonlarını sağlayan repository sınıfıdır.
    // ReadRepository<Product> → Generic ReadRepository’den kalıtım alır. 
    // Yani GetAll, GetWhere, GetSingleAsync, GetByIdAsync gibi ortak okuma metotları Product için hazır gelir.
    // IProductReadRepository → Bu entity’ye özgü (Product) okuma arayüzünü uygular. 
    // Böylece bağımlılıklar interface üzerinden çözülür (DI ile test edilebilirlik, gevşek bağlılık).
     
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        // ctor: DI (Dependency Injection) ile EticaretAPIDbContext dışarıdan verilir.
        // base(context) → Generic ReadRepository’nin constructor’ına context gönderilir.
        // Böylece bu sınıf EF Core context’ini kullanarak sorgularını çalıştırabilir.
        public ProductReadRepository(EticaretAPIDbContext context) : base(context)
        {
        }
    }
}
