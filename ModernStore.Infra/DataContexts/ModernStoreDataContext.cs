using ModernStore.Domain.Entities;
using ModernStore.Infra.Mappings;
using System.Data.Entity;

namespace ModernStore.Infra.DataContexts
{
    public class ModernStoreDataContext : DbContext
    {
        public ModernStoreDataContext()
            //: base("ModernStoreConnectionString")
            : base(@"Server=.\sqlexpress;Database=ModernStore;Trusted_Connection=True;")

        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Customer> DbCustomers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new OrderItemMap());
            modelBuilder.Configurations.Add(new OrderMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new UserMap());


        }
    }
}
