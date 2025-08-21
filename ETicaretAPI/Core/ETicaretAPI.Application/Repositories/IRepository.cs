using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
        //DbSet<T> bir koleksiyon/kapı gibi düşün: EF Core’un DbContext’i üzerinden o entity’e sorgu ve değişiklik yapmana yarar.

        //Asıl bağlantı ve yaşam döngüsü DbContext üzerindedir.DbSet sadece “o tabloyla çalış” demektir; bağlantıyı doğrudan o açmaz.
    }
}
