using Microsoft.EntityFrameworkCore;

namespace PMChecklist_PD_API.Models
{
    public class PCMhecklistContext : DbContext
    {
        public PCMhecklistContext(DbContextOptions<PCMhecklistContext> options) : base(options) { }

        public DbSet<GroupUsers> GroupUsers { get; set; } = default!;
        public DbSet<Users> Users { get; set; } = default!;
        public DbSet<Log> Logs { get; set; } = default!;
        public DbSet<AppConfig> AppConfig { get; set; } = default!;
        public DbSet<CheckLists> CheckLists { get; set; } = default!;
        public DbSet<CheckListOptions> CheckListOptions { get; set; } = default!;
        public DbSet<CheckListTypes> CheckListTypes { get; set; } = default!;
        public DbSet<DataTypes> DataTypes { get; set; } = default!;
        public DbSet<GroupTypeCheckLists> GroupTypeCheckLists { get; set; } = default!;
        public DbSet<GroupCheckListOptions> GroupCheckListOptions { get; set; } = default!;
        public DbSet<GroupPermission> GroupPermission { get; set; } = default!;
        public DbSet<Permissions> Permissions { get; set; } = default!;
        public DbSet<ExpectedResult> ExpectedResult { get; set; } = default!;
        public DbSet<Menu> Menu { get; set; } = default!;
        
    }
}
