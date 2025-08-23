using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;
using F = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Persistence.Repositories
{ 
    public class FileReadRepository : ReadRepository<F.File>, IFileReadRepository
    {
        public FileReadRepository(EticaretAPIDbContext context) : base(context)
        {
        }
    }
}
