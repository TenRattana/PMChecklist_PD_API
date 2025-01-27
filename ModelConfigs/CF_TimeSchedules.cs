using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_TimeSchedules : IEntityTypeConfiguration<TimeSchedules>
    {
        public void Configure(EntityTypeBuilder<TimeSchedules> builder)
        {
            builder.HasKey(t => t.ScheduleID);

            builder.Property(t => t.ScheduleID)
                .IsRequired();

            builder.Property(t => t.ScheduleName)
                .HasMaxLength(150);

            builder.Property(t => t.TypeSchedule)
                .HasMaxLength(50);

            builder.Property(t => t.Tracking)
                .IsRequired(false);

            builder.Property(t => t.Custom)
                .IsRequired(false);

            builder.Property(t => t.IsActive)
                .HasDefaultValue(true);
        }
    }
}
