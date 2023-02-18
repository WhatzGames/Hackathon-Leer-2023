using Microsoft.EntityFrameworkCore;

namespace JaKleingartenParadies.DB;

public class BotContext : DbContext
{
    public static readonly string ConnectionsString = "Data Source=bot1.sqlite";
    public BotContext() : base()
    {
        
    }
    
    public BotContext(DbContextOptions<BotContext> builderOptions) : base(builderOptions)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestTable>()
                    .HasKey(x => new { x.Uid });

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(ConnectionsString);
        }
    }
}