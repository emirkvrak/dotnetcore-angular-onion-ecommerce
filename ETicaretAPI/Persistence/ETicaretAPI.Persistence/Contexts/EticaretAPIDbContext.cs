using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Contexts
{
    public class EticaretAPIDbContext : DbContext
    {

        public EticaretAPIDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {




            var dates = ChangeTracker
                .Entries<BaseEntity>();

            foreach (var data in dates) 
            {
                _ = data.State switch
                {

                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow

                };
            }


            return base.SaveChangesAsync(cancellationToken);
        }
        





    }
}
