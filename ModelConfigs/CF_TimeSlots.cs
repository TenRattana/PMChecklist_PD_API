using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_TimeSlots : IEntityTypeConfiguration<TimeSlots>
    {
        public void Configure(EntityTypeBuilder<TimeSlots> builder)
        {
            builder.HasKey(t => t.TSlotID);

            builder.Property(t => t.TSlotID)
                .IsRequired();

            builder.Property(t => t.ScheduleID)
                .HasMaxLength(9);

            builder.Property(t => t.Day)
                .HasMaxLength(20);

            builder.Property(t => t.Start)
                .HasMaxLength(5);

            builder.Property(t => t.End)
                .HasMaxLength(5);

            builder.HasOne(t => t.TimeSchedule)
                .WithMany()
                .HasForeignKey(t => t.ScheduleID);
        }
    }
}
