using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JaKleingartenParadies.DB;

public class DBDesignTimeFactory : IDesignTimeDbContextFactory<BotContext>
{
    public BotContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BotContext>();
        builder.UseSqlite(BotContext.ConnectionsString);

        return new BotContext(builder.Options);
    }
}