using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_GroupTypeCheckList : IEntityTypeConfiguration<GroupTypeCheckLists>
    {
        public void Configure(EntityTypeBuilder<GroupTypeCheckLists> builder)
        {
            builder.HasKey(g => g.GTypeID);

            builder.Property(g => g.GTypeID)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(g => g.GTypeName)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(g => g.Icon)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(g => g.IsActive)
                .HasDefaultValue(true)
                .IsRequired(false);
        }
    }
}
