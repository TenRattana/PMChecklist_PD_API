using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_Users : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(u => u.UserID);

            builder.Property(u => u.UserID)
                .IsRequired();

            builder.Property(u => u.UserName)
                .HasMaxLength(150);

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(u => u.GroupUser)
                .WithMany()
                .HasForeignKey(u => u.GUserID);
        }
    }
}
