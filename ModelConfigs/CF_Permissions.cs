using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_Permissions : IEntityTypeConfiguration<Permissions>
    {
        public void Configure(EntityTypeBuilder<Permissions> builder)
        {
            builder.HasKey(p => p.PermissionID);

            builder.Property(p => p.PermissionID)
                .IsRequired();

            builder.Property(p => p.PermissionName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                .HasMaxLength(255);

            builder.Property(p => p.IsActive)
                .HasDefaultValue(true);
        }
    }
}
