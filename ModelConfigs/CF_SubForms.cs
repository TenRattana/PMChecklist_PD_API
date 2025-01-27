using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_SubForms : IEntityTypeConfiguration<SubForms>
    {
        public void Configure(EntityTypeBuilder<SubForms> builder)
        {
            builder.HasKey(s => s.SFormID);

            builder.Property(s => s.SFormID)
                .IsRequired();

            builder.Property(s => s.SFormName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Number)
                .IsRequired(false);

            builder.Property(s => s.Columns)
                .IsRequired();

            builder.Property(s => s.DisplayOrder)
                .IsRequired();

            builder.HasOne<Forms>()
                .WithMany()
                .HasForeignKey(s => s.FormID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
