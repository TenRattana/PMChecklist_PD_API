using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_GroupMachines : IEntityTypeConfiguration<GroupMachines>
    {
        public void Configure(EntityTypeBuilder<GroupMachines> builder)
        {
            builder.HasKey(g => g.GMachineID);

            builder.Property(g => g.GMachineID)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(g => g.GMachineName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(g => g.Description)
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(g => g.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasOne(g => g.TimeSchedule)
                .WithMany()
                .HasForeignKey(g => g.ScheduleID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
