using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_Forms : IEntityTypeConfiguration<Forms>
    {
        public void Configure(EntityTypeBuilder<Forms> builder)
        {
            builder.HasKey(f => f.FormID);

            builder.Property(f => f.FormID)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(f => f.FormNumber)
                .HasMaxLength(15)
                .IsRequired(false);

            builder.Property(f => f.FormName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(f => f.Description)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(f => f.IsActive)
                .HasDefaultValue(true)
                .IsRequired();
        }
    }
}
