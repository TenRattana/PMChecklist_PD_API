using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_Importants : IEntityTypeConfiguration<Importants>
    {
        public void Configure(EntityTypeBuilder<Importants> builder)
        {
            builder.HasKey(i => i.ImportantID);

            builder.Property(i => i.ImportantID)
                .IsRequired();

            builder.Property(i => i.Value)
                .HasMaxLength(9)
                .IsRequired(false);

            builder.Property(i => i.MinLength)
                .IsRequired(false);

            builder.Property(i => i.MaxLength)
                .IsRequired(false);

            builder.HasOne(i => i.MatchCheckList)
                .WithMany()
                .HasForeignKey(i => i.MCListID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
