namespace Models;

public class StockMarketContext : DbContext
{
    public virtual DbSet<Stock> Stocks { get; set; }
    public virtual DbSet<BoughtStock> BoughtStocks { get; set; }
    public virtual DbSet<Portfolio> Portfolios { get; set; }

    public StockMarketContext(DbContextOptions options) : base(options)
    {
           
    }
}
