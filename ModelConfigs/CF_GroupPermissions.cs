using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_GroupPermissions : IEntityTypeConfiguration<GroupPermission>
    {
        public void Configure(EntityTypeBuilder<GroupPermission> builder)
        {
            builder.HasKey(gp => new { gp.GuserID, gp.PermissionID });

            builder.HasOne(gp => gp.GroupUser)
                .WithMany()
                .HasForeignKey(gp => gp.GuserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(gp => gp.Permission)
                .WithMany()
                .HasForeignKey(gp => gp.PermissionID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(gp => gp.GuserID)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(gp => gp.PermissionID)
                .IsRequired();

            builder.Property(gp => gp.PermissionStatus)
                .IsRequired(false);

            builder.Property(gp => gp.IsActive)
                .HasDefaultValue(true)
                .IsRequired(false);

            builder.Property(gp => gp.CreateDate)
                .HasDefaultValueSql("getdate()")
                .IsRequired(false);
        }
    }
}
