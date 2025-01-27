using Microsoft.EntityFrameworkCore;
using PMChecklist_PD_API.Models;

namespace PMChecklist_PD_API.Models;

public class PCMhecklistContext : DbContext
{
    public PCMhecklistContext(DbContextOptions<PCMhecklistContext> options) : base(options) { }

    public DbSet<GroupUsers> GroupUsers { get; set; } = default!;
    public DbSet<Users> Users { get; set; } = default!;
    public DbSet<Log> Logs { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>()
            .HasOne(u => u.GroupUser) 
            .WithMany()   
            .HasForeignKey(u => u.GUserID);
    }
}