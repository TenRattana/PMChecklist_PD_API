using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_GroupUsers : IEntityTypeConfiguration<GroupUsers>
    {
        public void Configure(EntityTypeBuilder<GroupUsers> builder)
        {
            builder.HasKey(g => g.GUserID);

            builder.Property(g => g.GUserID)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(g => g.GUserName)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(g => g.IsActive)
                .HasDefaultValue(true)
                .IsRequired(false);
        }
    }
}
