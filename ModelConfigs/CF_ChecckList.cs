using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_CheckLists : IEntityTypeConfiguration<CheckLists>
    {
        public void Configure(EntityTypeBuilder<CheckLists> builder)
        {
            builder.HasKey(c => c.CListID);

            builder.Property(c => c.CListID)
                .IsRequired()   
                .HasMaxLength(9); 

            builder.Property(c => c.CListName)
                .IsRequired()   
                .HasMaxLength(150); 

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true)  
                .IsRequired(false); 
        }
    }
}
