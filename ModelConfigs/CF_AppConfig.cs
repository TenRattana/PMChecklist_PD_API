using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_AppConfig : IEntityTypeConfiguration<AppConfig>
    {
        public void Configure(EntityTypeBuilder<AppConfig> builder)
        {
            builder.HasKey(a => a.AppID);

            builder.Property(a => a.AppID)
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(a => a.AppName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.GroupMachine)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_GroupMachine)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.Machine)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_Machine)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.CheckList)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_CheckList)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.GroupCheckList)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_GroupCheckList)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.CheckListOption)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_CheckListOption)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.MatchCheckListOption)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_MatchCheckListOption)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.MatchFormMachine)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_MatchFormMachine)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.Form)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_Form)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.SubForm)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_SubForm)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.ExpectedResult)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_ExpectedResult)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.UsersPermission)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_UsersPermission)
                .HasMaxLength(9)
                .IsRequired();

            builder.Property(a => a.TimeSchedule)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.PF_TimeSchedule)
                .HasMaxLength(9)
                .IsRequired();
        }
    }
}
