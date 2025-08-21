using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ETicaretAPI.Persistence.Repositories
{
    // Generic bir okuma repository’si.
    // IReadRepository<T> arayüzünü uygular ve sadece okuma işlemlerini kapsar.
    // T, BaseEntity’den türemiş olmalıdır (Id, CreatedDate, UpdatedDate gibi ortak property'leri vardır).
    public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
    {
        private readonly EticaretAPIDbContext _context;

        // Constructor’da DbContext dependency injection ile alınır.
        public ReadRepository(EticaretAPIDbContext context)
        {
            _context = context;
        }

        // EF Core’da her tablo DbSet<T> ile temsil edilir.
        // _context.Set<T>() => generic olarak T tipine karşılık gelen tabloyu verir.
        // Böylece Product, Order, Customer gibi her entity için ortak çalışabilir.
        public DbSet<T> Table => _context.Set<T>();


        // Tablodaki tüm kayıtları sorgulamak için kullanılır.
        // IQueryable<T> döner: SQL sorgusu hemen çalışmaz, üzerine Where/OrderBy eklenebilir.
        // tracking parametresi:
        //   true  -> EF Core entity'leri Change Tracker’a ekler (Update/Delete yapabilirsin).
        //   false -> AsNoTracking() ile sadece okunur, performanslıdır.
        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable(); // IQueryable oluşturulur.
            if (!tracking)
                query = query.AsNoTracking(); // Tracking kapatılır.
            return query;
        }

        // Belirli bir şarta uyan kayıtları getirir.
        // Parametre: Expression<Func<T,bool>> -> lambda ifadesini alır (örn: p => p.Price > 100).
        // IQueryable döner: sorgu zincirlenebilir.
        // tracking: true/false yine performans kontrolü için.
        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method); // SQL'e çevrilebilir filtre.
            return tracking ? query : query.AsNoTracking();
        }

        // Şarta uyan tek bir kaydı asenkron getirir.
        // Task<T> döner çünkü DB’ye gidip veri çeker (I/O -> async).
        // İlk uyan kaydı getirir, bulamazsa null döner.
        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable(); // IQueryable başlangıcı
            if (!tracking)
                query = query.AsNoTracking(); // Performans için takip kapatılabilir
            return await query.FirstOrDefaultAsync(method); // SQL çalışır, async beklenir
        }

        // Primary Key (Id) ile tek bir kaydı getirir.
        // Parametre string çünkü BaseEntity.Id genellikle Guid tutulur ama dışarıdan string gelir.
        // Guid.Parse ile parse edilerek DB sorgusu yapılır.
        // tracking yine opsiyonel.
        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            // FirstOrDefaultAsync: Id eşleşirse döner, bulamazsa null.
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }
    }
}
