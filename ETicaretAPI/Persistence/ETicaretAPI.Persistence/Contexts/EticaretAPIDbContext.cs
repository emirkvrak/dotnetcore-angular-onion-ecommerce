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

            foreach (var date in dates) 
            {
                if (date.State == EntityState.Added)
                {
                    date.Entity.CreatedDate = DateTime.UtcNow;
                }
                else if (date.State == EntityState.Modified)
                {
                    date.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }


            return base.SaveChangesAsync(cancellationToken);
        }
        





    }
}
