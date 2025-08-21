using ETicaretAPI.Domain.Entities.Common;
using System.Linq.Expressions;

namespace ETicaretAPI.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity
    {
        // Tablonun tamamına sorgu başlatmak için kullanılır.
        // Dönüş: IQueryable<T> → Ertelenmiş yürütme (deferred); tüketen katman Where/OrderBy/Select ekleyip
        //        tek SQL ile çalıştırabilsin diye sorgu nesnesi döndürülür (henüz DB’ye gitmez).
        // Parametre: tracking → EF Core Change Tracker açık mı kapalı mı?
        //            true: entity'ler takip edilir (sonradan Update/Delete yapacaksan lazım),
        //            false: AsNoTracking; salt-okuma için daha performanslı.
        IQueryable<T> GetAll(bool tracking = true);

        // Belirli bir şarta uyan kayıtlar için sorgu başlatır.
        // Dönüş: IQueryable<T> → Şart(lar) kompoze edilebilsin, gerekiyorsa Include/OrderBy eklenebilsin.
        // Parametre: Expression<Func<T,bool>> method → Lambda ifadesi ağaç olarak alınır,
        //            EF bunu SQL'e çevirebilir (server-side filtreleme, performans).
        // Parametre: tracking → Yukarıdaki ile aynı mantık.
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);

        // Şarta uyan TEK bir kaydı getirir (ilkini).
        // Dönüş: Task<T> → Veritabanına gerçekten gider (I/O) → async/await gerekli.
        // Parametre: Expression<Func<T,bool>> method → Dinamik şart; SQL'e çevrilir.
        // Parametre: tracking → Değişiklik yapılacaksa true, salt-okuma ise false önerilir.
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);

        // Id'ye göre TEK bir kaydı getirir.
        // Dönüş: Task<T> → Veritabanına gider (I/O) → async/await.
        // Parametre: string id → BaseEntity.Id çoğu projede Guid/string olur; dışarıyla uyum için string alınır,
        //            içeride Guid.Parse vs. yapılabilir.
        // Parametre: tracking → Okuma senaryosunda çoğunlukla false performans için tercih edilir.
        Task<T> GetByIdAsync(string id, bool tracking = true);
    }
}
