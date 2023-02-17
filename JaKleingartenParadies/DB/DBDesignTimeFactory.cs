using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JaKleingartenParadies.DB;

public class DBDesignTimeFactory : IDesignTimeDbContextFactory<BotContext>
{
    public BotContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BotContext>();
        builder.UseSqlite("./db.sqlite");

        return new BotContext(builder.Options);
    }
}