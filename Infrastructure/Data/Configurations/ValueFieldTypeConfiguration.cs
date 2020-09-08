using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ValueFieldTypeConfiguration : IEntityTypeConfiguration<ValueFieldType>
    {
        public void Configure(EntityTypeBuilder<ValueFieldType> builder)
        {
            builder.ToTable("ValueFieldTypes");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id).UseIdentityColumn();

            builder.Property(t => t.Name).HasMaxLength(100);

            builder.Property(t => t.Description).HasMaxLength(200);

            builder.HasAlternateKey(t => t.Name).HasName("UniqueKey_ValueFieldTypeName");
        }
    }
}
