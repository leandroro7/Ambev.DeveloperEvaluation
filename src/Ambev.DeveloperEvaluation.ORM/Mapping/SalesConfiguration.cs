using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class SalesConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                   .HasColumnType("uuid")
                   .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.SaleDate)
                   .IsRequired();

            builder.Property(s => s.TotalAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Since SaleItem does not have a navigation property to Sale, configure the relation using a shadow foreign key.
            builder.HasMany(s => s.Items)
                   .WithOne()
                   .HasForeignKey("SaleId");
        }
    }
}
