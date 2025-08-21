using ETicaretAPI.Domain.Entities.Common;

namespace ETicaretAPI.Application.Repositories
{
    public interface IWriteRepository<T> : IRepository<T> where T : BaseEntity
    {
        // Yeni bir entity eklemek için kullanılır.
        // Parametre: Eklenecek tek bir T tipinde entity.
        // Dönüş tipi: Task<bool> -> Asenkron ekleme işlemi yapılır, 
        //              true/false ile işlemin başarılı olup olmadığı bildirilir.
        Task<bool> AddAsync(T model);

        // Birden fazla entity eklemek için kullanılır.
        // Parametre: Eklenecek entity'lerin listesi.
        // Dönüş tipi: Task<bool> -> Asenkron çalışır, tümünün eklenip eklenmediği bilgisi döner.
        Task<bool> AddRangeAsync(List<T> datas);

        // Id üzerinden entity silmek için kullanılır.
        // Parametre: string id -> BaseEntity içinde Id alanı genelde string/Guid olduğu için böyle tanımlanmıştır.
        // Dönüş tipi: Task<bool> -> Veritabanından asenkron olarak bulup silmesi gerektiği için Task,
        //              başarılı/başarısız sonucu için bool.
        Task<bool> RemoveAsync(string id);

        // Entity referansı üzerinden silmek için kullanılır.
        // Parametre: Silinecek entity nesnesi.
        // Dönüş tipi: bool -> Burada asenkron yok, çünkü EF Core sadece entity'nin durumunu Deleted yapar,
        //              veritabanına gitmez. Bu yüzden senkron bool.
        bool Remove(T model);

        // Birden fazla entity silmek için kullanılır.
        // Parametre: Silinecek entity'lerin listesi.
        // Dönüş tipi: bool -> Yukarıdaki mantıkla aynı, sadece birden fazla entity'yi işaretler.
        bool RemoveRange(List<T> datas);

        // Güncelleme için kullanılır.
        // Parametre: Güncellenecek entity nesnesi.
        // Dönüş tipi: bool -> EF Core entity'nin durumunu Modified yapar, veritabanına hemen gitmez. 
        //              Gerçek yazma SaveAsync ile yapılır.
        bool Update(T model);

        // Gerçek veritabanı işlemlerini (insert, update, delete) kalıcı hale getirir.
        // Parametre: yok -> çünkü context'teki değişiklikleri topluca kaydeder.
        // Dönüş tipi: Task<int> -> Asenkron SaveChanges çağrısı yapılır.
        //              int: Etkilenen satır sayısını döner.
        Task<int> SaveAsync();
    }

}
