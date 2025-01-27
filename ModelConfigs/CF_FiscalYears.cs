using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PMChecklist_PD_API.Models.Configurations
{
    public class CF_FiscalYear : IEntityTypeConfiguration<FiscalYears>
    {
        public void Configure(EntityTypeBuilder<FiscalYears> builder)
        {
            builder.HasKey(f => f.FiscalID);

            builder.Property(f => f.FiscalID)
                .IsRequired()
                .HasMaxLength(9);
            builder.Property(f => f.FiscalYear)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
