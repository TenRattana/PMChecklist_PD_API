using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_MatchCheckListOption : IEntityTypeConfiguration<MatchCheckListOption>
    {
        public void Configure(EntityTypeBuilder<MatchCheckListOption> builder)
        {
            builder.HasKey(mclo => mclo.MCLOptionID);

            builder.Property(mclo => mclo.MCLOptionID)
                .IsRequired();

            builder.Property(mclo => mclo.IsActive)
                .HasDefaultValue(true);

            builder.Property(mclo => mclo.DisplayOrder)
                .HasColumnType("tinyint");

            builder.HasOne(mclo => mclo.GroupCheckListOption)
                .WithMany()
                .HasForeignKey(mclo => mclo.GCLOptionID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(mclo => mclo.CheckListOption)
                .WithMany()
                .HasForeignKey(mclo => mclo.CLOptionID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
