using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_GroupCheckListOption : IEntityTypeConfiguration<GroupCheckListOptions>
    {
        public void Configure(EntityTypeBuilder<GroupCheckListOptions> builder)
        {
            builder.HasKey(g => g.GCLOptionID);

            builder.Property(g => g.GCLOptionID)
                .HasMaxLength(9) 
                .IsRequired(); 

            builder.Property(g => g.GCLOptionName)
                .HasMaxLength(150) 
                .IsRequired(false);  

            builder.Property(g => g.IsActive)
                .HasDefaultValue(true)  
                .IsRequired();
        }
    }
}
