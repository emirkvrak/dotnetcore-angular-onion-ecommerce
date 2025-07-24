using ETicaretAPI.Domain.Entities.Common;

namespace ETicaretAPI.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> datas);
        Task<bool> Remove(string id);
        bool Remove(T model);
        bool RemoveRange(List<T> datas);
        bool Update(T model);
        Task<int> SaveAsync();

    }
}
