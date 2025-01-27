using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_CheckListTypes : IEntityTypeConfiguration<CheckListTypes>
    {
        public void Configure(EntityTypeBuilder<CheckListTypes> builder)
        {
            builder.HasKey(c => c.CTypeID);

            builder.Property(c => c.CTypeID)
                .IsRequired()
                .HasMaxLength(9);

            builder.Property(c => c.GTypeID)
                .HasMaxLength(9);

            builder.Property(c => c.CTypeName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.CTypeTitle)
                .HasMaxLength(150);

            builder.Property(c => c.Displayorder)
                .IsRequired(false);

            builder.Property(c => c.Icon)
                .HasMaxLength(150);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true)
                .IsRequired(false);

            builder.HasOne(c => c.GroupTypeCheckLists)
                .WithMany()
                .HasForeignKey(c => c.GTypeID)
                .HasConstraintName("FK_CheckListTypes_CheckListTypes")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
