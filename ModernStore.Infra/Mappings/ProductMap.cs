using ModernStore.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace ModernStore.Infra.Mappings
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Product");
            HasKey(x => x.Id);

            Property(x => x.Id);
            Property(x => x.Image).IsRequired().HasMaxLength(1024);
            Property(x => x.Price).HasColumnType("money");
            Property(x => x.QuantityHand);
            Property(x => x.Title).IsRequired().HasMaxLength(80);
        }
    }
}
