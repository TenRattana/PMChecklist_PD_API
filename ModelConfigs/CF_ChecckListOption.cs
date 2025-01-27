using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_CheckListOptions : IEntityTypeConfiguration<CheckListOptions>
    {
        public void Configure(EntityTypeBuilder<CheckListOptions> builder)
        {
            builder.HasKey(c => c.CLOptionID);

            builder.Property(c => c.CLOptionID)
                .IsRequired()    
                .HasMaxLength(9); 

            builder.Property(c => c.CLOptionName)
                .IsRequired()   
                .HasMaxLength(150); 

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true)  
                .IsRequired(false); 
        }
    }
}
