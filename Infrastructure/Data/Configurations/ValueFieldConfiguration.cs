using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ValueFieldConfiguration : IEntityTypeConfiguration<ValueField>
    {
        public void Configure(EntityTypeBuilder<ValueField> builder)
        {
            builder.ToTable("ValueFields");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id).UseIdentityColumn();

            builder.Property(e => e.Name).HasMaxLength(100);

            builder.Property(e => e.Description).HasMaxLength(200);

            builder.HasOne(e => e.ValueFieldType)
                .WithMany(e => e.ValueFields)
                .HasForeignKey(e => e.TypeId)
                .HasConstraintName("FK_ValueField_ValueFieldType")
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
