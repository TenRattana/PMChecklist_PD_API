using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_Menu : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(m => m.MenuID);

            builder.Property(m => m.MenuID)
                .IsRequired();

            builder.Property(m => m.MenuPermission)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.MenuLabel)
                .HasMaxLength(50);

            builder.Property(m => m.Description)
                .HasMaxLength(255);

            builder.Property(m => m.Path)
                .HasMaxLength(150);

            builder.Property(m => m.NavigationTo)
                .HasMaxLength(50);

            builder.Property(m => m.Icon)
                .HasMaxLength(150);

            builder.Property(m => m.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(m => m.Permission)
                .WithMany()
                .HasForeignKey(m => m.PermissionID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
