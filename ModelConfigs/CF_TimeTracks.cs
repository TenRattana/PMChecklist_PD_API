using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_TimeTrack : IEntityTypeConfiguration<TimeTracks>
    {
        public void Configure(EntityTypeBuilder<TimeTracks> builder)
        {
            builder.HasKey(t => t.TTrackID);

            builder.Property(t => t.TTrackID)
                .IsRequired();

            builder.Property(t => t.ScheduleID)
                .HasMaxLength(9);

            builder.Property(t => t.Start)
                .IsRequired();

            builder.Property(t => t.Stop)
                .IsRequired();

            builder.HasOne(t => t.TimeSchedule)
                .WithMany()
                .HasForeignKey(t => t.ScheduleID);
        }
    }
}
