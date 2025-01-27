using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_TimeCustoms : IEntityTypeConfiguration<TimeCustoms>
    {
        public void Configure(EntityTypeBuilder<TimeCustoms> builder)
        {
            builder.HasKey(t => t.TCustomID);

            builder.Property(t => t.TCustomID)
                .IsRequired();

            builder.Property(t => t.Start)
                .IsRequired(false)
                .HasMaxLength(20);

            builder.Property(t => t.End)
                .IsRequired(false)
                .HasMaxLength(20);

            builder.HasOne<TimeSchedules>()
                .WithMany()
                .HasForeignKey(t => t.ScheduleID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
