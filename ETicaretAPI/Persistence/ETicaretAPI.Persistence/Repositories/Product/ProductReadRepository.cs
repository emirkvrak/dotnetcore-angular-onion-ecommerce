using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        public ProductReadRepository(EticaretAPIDbContext context) : base(context) // base miras alınnan sınıfın(parent) constructorını çalıştırır base.X methoduna erişir
        {
        }
    }
}
