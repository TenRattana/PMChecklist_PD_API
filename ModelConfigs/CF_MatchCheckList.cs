using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_MatchCheckList : IEntityTypeConfiguration<MatchCheckList>
    {
        public void Configure(EntityTypeBuilder<MatchCheckList> builder)
        {
            builder.HasKey(mcl => mcl.MCListID);

            builder.Property(mcl => mcl.MCListID)
                .IsRequired(); 

            builder.Property(mcl => mcl.Required)
                .HasDefaultValue(false);  

            builder.Property(mcl => mcl.DisplayOrder)
                .IsRequired();  

            builder.Property(mcl => mcl.MinLength)
                .HasColumnType("float");

            builder.Property(mcl => mcl.MaxLength)
                .HasColumnType("float");

            builder.Property(mcl => mcl.Description)
                .HasColumnType("nvarchar(max)");

            builder.Property(mcl => mcl.Placeholder)
                .HasMaxLength(150);

            builder.Property(mcl => mcl.Hint)
                .HasMaxLength(150);

            builder.HasOne(mcl => mcl.CheckList)
                .WithMany()
                .HasForeignKey(mcl => mcl.CListID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(mcl => mcl.GroupCheckListOption)
                .WithMany()
                .HasForeignKey(mcl => mcl.GCLOptionID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(mcl => mcl.CheckListType)
                .WithMany()
                .HasForeignKey(mcl => mcl.CTypeID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(mcl => mcl.DataType)
                .WithMany()
                .HasForeignKey(mcl => mcl.DTypeID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(mcl => mcl.SubForm)
                .WithMany()
                .HasForeignKey(mcl => mcl.SFormID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
