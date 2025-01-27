using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_Machine : IEntityTypeConfiguration<Machines>
    {
        public void Configure(EntityTypeBuilder<Machines> builder)
        {
            builder.HasKey(m => m.MachineID);

            builder.Property(m => m.MachineID)
                .IsRequired();
            builder.Property(m => m.MachineName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.MachineCode)
                .HasMaxLength(50);

            builder.Property(m => m.Building)
                .HasMaxLength(150);

            builder.Property(m => m.Floor)
                .HasMaxLength(150);

            builder.Property(m => m.Area)
                .HasMaxLength(150);

            builder.Property(m => m.Description)
                .HasMaxLength(255);

            builder.Property(m => m.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(m => m.Form)
                .WithMany()
                .HasForeignKey(m => m.FormID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.GroupMachine)
                .WithMany()
                .HasForeignKey(m => m.GMachineID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
