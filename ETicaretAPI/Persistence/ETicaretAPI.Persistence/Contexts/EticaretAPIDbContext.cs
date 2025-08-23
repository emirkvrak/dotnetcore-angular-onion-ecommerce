using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Contexts
{
    public class EticaretAPIDbContext : DbContext
    {
        // DbContext dışarıdan konfigüre edilen options ile kurulur (connection string, provider vb.)
        public EticaretAPIDbContext(DbContextOptions options) : base(options) { }

        // EF Core tabloları: her DbSet<T> veritabanında bir tabloyu temsil eder
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<ProductImageFile> ProductImageFile { get; set; }
        public DbSet<InvoiceFile> InvoiceFile { get; set; }


        // SaveChangesAsync: EF Core'un değişiklikleri DB'ye yazdığı noktadır (INSERT/UPDATE/DELETE)
        // Burayı override ederek "otomatik tarih atamayı" merkezi olarak uygularız.
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // ChangeTracker: Context içindeki tüm entity'lerin durumunu (Added/Modified/Deleted/Unchanged) takip eder.
            // BaseEntity olan tüm entry'leri yakalıyoruz ki CreatedDate/UpdatedDate alanlarını yönetelim.
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                // EntityState'e göre alanları set et (UTC kullanmak: saat dilimi sorunlarını önler).
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Yeni eklenen kayıtlarda CreatedDate set edilir.
                        entry.Entity.CreatedDate = DateTime.UtcNow;

                        // İyileştirme: CreatedDate sonradan yanlışlıkla güncellenmesin
                        // (Added durumda ihtiyaç yok ama güvenlik amaçlı aşağıdaki satır çoğu projede kullanılır)
                        entry.Property(x => x.CreatedDate).IsModified = false;
                        break;

                    case EntityState.Modified:
                        // Güncellenen kayıtlarda UpdatedDate set edilir.
                        entry.Entity.UpdatedDate = DateTime.UtcNow;

                        // İyileştirme: Update sırasında CreatedDate asla değişmesin
                        entry.Property(x => x.CreatedDate).IsModified = false;
                        break;

                    // Diğer durumlarda (Deleted/Unchanged/Detached) tarih atamayız.
                    // Soft-delete senaryosu varsa burada IsDeleted/DeletedDate set edilebilir.
                    default:
                        break;
                }
            }

            // Asıl veritabanı yazma burada olur (async I/O)
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
