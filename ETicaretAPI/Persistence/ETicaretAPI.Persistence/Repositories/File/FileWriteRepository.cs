using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;
using F = ETicaretAPI.Domain.Entities;

namespace ETicaretAPI.Persistence.Repositories
{
    public class FileWriteRepository : WriteRepository<F.File>, IFileWriteRepository
    {
        public FileWriteRepository(EticaretAPIDbContext context) : base(context)
        {
        }
    }
}
