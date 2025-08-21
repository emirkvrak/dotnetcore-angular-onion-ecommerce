using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;
using ETicaretAPI.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ETicaretAPI.Persistence
{
    // IServiceCollection için bir "extension method" tanımlıyoruz.
    // Amaç: Persistence (EF Core + Repo) bağımlılıklarını tek yerden kaydetmek.
    public static class ServiceRegistration
    {
        // Program.cs içinde: services.AddPersistenceServices(); şeklinde çağrılır.
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            // DbContext kaydı:
            // - EticaretAPIDbContext EF Core context'imiz.
            // - UseNpgsql: PostgreSQL sağlayıcısını kullan (Npgsql).
            // - Configuration.ConnectionString: appsettings.json (veya environment) üzerinden gelen connection string.
            //
            // Lifetime: AddDbContext varsayılan olarak Scoped (request başına bir context) ekler.
            // Neden Scoped? DbContext "unit of work" olarak request boyunca tek sefer kullanılır;
            // thread-safe değildir, Singleton yapmak doğru değildir.
            services.AddDbContext<EticaretAPIDbContext>(options =>
                options.UseNpgsql(Configuration.ConnectionString)
            // İYİ PRATİKLER (opsiyonel):
            // .EnableSensitiveDataLogging()   // sadece Development'ta aç (PII loglar!)
            // .EnableDetailedErrors()         // detaylı hata mesajları (dev)
            // .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // global no-tracking (read ağırlıklı projelerde)
            // .EnableRetryOnFailure()         // transient hata dayanıklılığı (SQL Azure vs.)
            );

            // Açıklama:
            // "Benim DbContext’im EticaretAPIDbContext. Sağlayıcı olarak PostgreSQL kullanıyorum."
            // UseNpgsql → EF Core'un Npgsql sağlayıcısını aktive eder ve bağlantıyı yapılandırır.
            // Connection string → appsettings.json / secrets / environment değişkenlerinden okunur.

            // Repository kayıtları:
            // Hepsi Scoped: her HTTP isteğinde yeni instance (DbContext ile aynı scope).
            // Bu sayede aynı request boyunca tek DbContext paylaşılır (transaction & change tracker tutarlıdır).

            // Order
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();     // Okuma operasyonları (IQueryable, async materialize)
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();   // Yazma operasyonları (Add/Update/Remove + SaveAsync)

            // Product
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            // Customer
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();


            // Notlar:
            // - Interface → Concrete eşlemesi DI ile gevşek bağlılık sağlar (test edilebilirlik, mock).
            // - Scoped seçimi, DbContext'in scope'u ile uyumludur; Singleton seçilmemelidir.
            // - Transient de kullanılabilir ama request içinde birden çok context/transaction parçasına yol açabilir.
        }
    }
}
