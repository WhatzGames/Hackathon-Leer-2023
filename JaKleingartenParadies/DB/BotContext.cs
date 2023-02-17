using Microsoft.EntityFrameworkCore;

namespace JaKleingartenParadies.DB;

public class BotContext : DbContext
{
    public BotContext() : base()
    {
        
    }
    
    public BotContext(DbContextOptions<BotContext> builderOptions) : base(builderOptions)
    {
        
    }
}