using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_DataTypes : IEntityTypeConfiguration<DataTypes>
    {
        public void Configure(EntityTypeBuilder<DataTypes> builder)
        {
            builder.HasKey(d => d.DTypeID);

            builder.Property(d => d.DTypeID)
                .IsRequired()
                .HasMaxLength(9);

            builder.Property(d => d.DTypeName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(d => d.Icon)
                .HasMaxLength(150);

            builder.Property(d => d.IsActive)
                .HasDefaultValue(true)
                .IsRequired(false);
        }
    }
}
