using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_ExpectedResults : IEntityTypeConfiguration<ExpectedResult>
    {
        public void Configure(EntityTypeBuilder<ExpectedResult> builder)
        {
            builder.HasKey(er => er.EResultID);

            builder.Property(er => er.EResultID)
                .ValueGeneratedOnAdd();

            builder.Property(er => er.MCListID)
                .IsRequired(false);

            builder.Property(er => er.FormID)
                .HasMaxLength(9);

            builder.Property(er => er.MachineID)
                .HasMaxLength(9);

            builder.Property(er => er.GCLOptionID)
                .HasMaxLength(9);

            builder.Property(er => er.UserID)
                .HasMaxLength(9);

            builder.Property(er => er.TableID)
                .HasMaxLength(9);

            builder.Property(er => er.EResult)
                .HasMaxLength(255);

            builder.Property(er => er.ApprovedID)
                .HasMaxLength(9);

            builder.Property(er => er.Status)
                .HasDefaultValue(true)
                .IsRequired(false);

            builder.Property(er => er.ApprovedTime)
                .IsRequired(false);

            builder.Property(er => er.CreateDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasOne<Forms>()
                .WithMany()
                .HasForeignKey(er => er.FormID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Machines>()
                .WithMany()
                .HasForeignKey(er => er.MachineID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<MatchCheckList>()
                .WithMany()
                .HasForeignKey(er => er.MCListID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
