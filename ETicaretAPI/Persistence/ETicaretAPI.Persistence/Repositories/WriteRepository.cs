using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities.Common;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ETicaretAPI.Persistence.Repositories
{
    // Generic yazma (command) repository'si.
    // Yalnızca ekleme/silme/güncelleme ve değişiklikleri kaydetme sorumluluğunu taşır.
    // T -> BaseEntity (Id/CreatedDate/UpdatedDate gibi ortak alanlar) şartı ile çalışır.
    public class WriteRepository<T> : IWriteRepository<T> where T : BaseEntity
    {
        private readonly EticaretAPIDbContext _context;

        // DbContext DI ile dışarıdan gelir (connection, provider, tracking ayarları vs. bu context'tedir).
        public WriteRepository(EticaretAPIDbContext context)
        {
            _context = context;
        }

        // İlgili entity'nin DbSet'i. EF Core'da her tablo DbSet<T> ile temsil edilir.
        // _context.Set<T>() -> T tipinin tablosuna erişim sağlar (ör. Products, Orders...).
        public DbSet<T> Table => _context.Set<T>();


        // Tek bir entity eklemek için.
        // Dönüş tipi: Task<bool> → AddAsync I/O başlatmaz; ama EF bu API'yi async sunar (identity/generator senaryoları için).
        // Burada bool, "state Added olarak işaretlendi mi" diye anlık geri bildirim verir.
        // Asıl veritabanına yazma SaveAsync ile olur.
        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model); // EntityEntry: EF'nin bu entity'yi nasıl izlediği (State, CurrentValues vs.)
            return entityEntry.State == EntityState.Added;            // State burada "Added" ise ekleme için işaretlenmiştir.
        }

        // Çoklu ekleme (batch) için.
        // AddRangeAsync yine EF tarafında state'leri "Added" yapar; veritabanına henüz gitmez.
        // Büyük hacimlerde gerçek batch/bulk araçları (EF Bulk, TVP, COPY) daha performanslı olabilir.
        public async Task<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true; // Hepsi Added state'e işaretlenmiştir; hata varsa exception fırlatılır.
        }

        // Id ile silme. Önce entity bulunur (async I/O), sonra Remove ile Deleted state'e işaretlenir.
        // Dönüş: bool -> Remove(model) sonucunu döndürür (Deleted olarak işaretlendi mi).
        // Not: Bulunamazsa model null olur; Remove(null) exception atar. Bu nedenle null kontrolü iyi bir pratiktir.
        public async Task<bool> RemoveAsync(string id)
        {
            T model = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            // İyileştirme (öneri):
            // if (model is null) return false; // veya throw NotFound
            return Remove(model);
        }

        // Entity referansı ile silme. Veritabanına gitmez; sadece "Deleted" state olarak işaretler.
        // Asıl silme SaveAsync çağrıldığında olur.
        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);           // Remove: State -> Deleted
            return entityEntry.State == EntityState.Deleted;
        }

        // Çoklu silme. Yine veritabanına gitmez; SaveAsync'te fiziksel silinir.
        public bool RemoveRange(List<T> datas)
        {
            Table.RemoveRange(datas); // Listedeki tüm entity'ler Deleted state'e alınır.
            return true;
        }

        // Güncelleme. Veritabanına gitmez; "Modified" state olarak işaretler.
        // Değiştirilen property'ler SaveAsync'te UPDATE olarak yazılır.
        // EntityEntry: EF'nin takip bilgileri (State, Orijinal/Current değerler, Property bazlı IsModified vs.)
        public bool Update(T model)
        {
            EntityEntry<T> entityEntry = Table.Update(model); // Update: State -> Modified
            // İyileştirme (öneri): Sadece değişen alanları işaretlemek için:
            // _context.Entry(model).Property(x => x.CreatedDate).IsModified = false; vb.
            return entityEntry.State == EntityState.Modified;  // Şu anda "Modified" olarak işaretli mi?
        }

        // Gerçek veritabanı işlemi (INSERT/UPDATE/DELETE) burada yapılır.
        // EF Core değişikliklerini tek bir transaction altında DB'ye yazar.
        // Dönüş: int -> Etkilenen satır sayısı.
        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();
    }
}
