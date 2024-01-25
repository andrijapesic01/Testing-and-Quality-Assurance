namespace StockMarket.Controllers;

[ApiController]
[Route("[controller]")]
public class PortfolioController : ControllerBase
{
    public StockMarketContext Context { get; set; }

    public PortfolioController(StockMarketContext context)
    {
        Context = context;
    }

    [HttpPost("AddPortfolio")]
    public async Task<ActionResult<Portfolio>> AddPortfolio([FromBody] PortfolioModel pm)
    {
        var portfolio = new Portfolio 
        {
            OwnerName = pm.OwnerName,
            BankName = pm.BankName,
            BankBalance = pm.BankBalance,
            RiskTolerance = pm.RiskTolerance,
            InvestmentStrategy = pm.InvestmentStrategy
        };

        Context.Portfolios.Add(portfolio);

        await Context.SaveChangesAsync();
        return Ok(portfolio);
    }

    [HttpGet("GetPortfolio/{id}")]
    public async Task<ActionResult<Portfolio>> GetPortfolio(int id)
    {
        //var portfolio = await Context.Portfolios.FindAsync(id);
        var portfolio = await Context.Portfolios
            .Include(p => p.BoughtStocks)
            .ThenInclude(bs => bs.Stock)
            .FirstOrDefaultAsync(p => p.ID == id);

        if(portfolio == null)
            return NotFound("Portfolio with id " + id + " does not exist!");

        var ret = new {
            portfolio,
            portfolio.BoughtStocks
        };

        return Ok(ret);
        //return Ok(portfolio);
    }

    [HttpGet("GetAllPortfolios")]
    public async Task<ActionResult<IEnumerable<Portfolio>>> GetAllPortfolios()
    {
        var portfolios = await Context.Portfolios.ToListAsync();
        return Ok(portfolios);
    }

    [HttpPut("UpdatePortfolio/{id}")]
    public async Task<ActionResult<Stock>> UpdatePortfolio(int id, [FromBody] PortfolioModel pm)
    {
        var portfolio = await Context.Portfolios.FindAsync(id);
        if (portfolio == null)
        {
            return NotFound();
        }

        portfolio.OwnerName = pm.OwnerName;
        portfolio.BankBalance = pm.BankBalance;
        portfolio.BankBalance = pm.BankBalance;
        portfolio.RiskTolerance = pm.RiskTolerance;
        portfolio.InvestmentStrategy = pm.InvestmentStrategy;

        await Context.SaveChangesAsync();

        return Ok(portfolio);
    }

    [HttpDelete("DeletePortfolio/{id}")]
    public async Task<ActionResult> DeletePortfolio(int id)
    {
        // var portfolio = await Context.Portfolios.FindAsync(id);
        // if (portfolio == null)
        //     return NotFound("Portfolio with id " + id + " does not exist!");

    var portfolio = await Context.Portfolios
        .Include(p => p.BoughtStocks)
        .FirstOrDefaultAsync(p => p.ID == id);

        if (portfolio != null)
        {
            portfolio.BoughtStocks.Clear();

            // Remove the Portfolio
            Context.Portfolios.Remove(portfolio);

            // Save changes
            await Context.SaveChangesAsync();
            return Ok("Portfolio with id " + id + " successfully deleted!");
        }

        // Context.Portfolios.Remove(portfolio);
        // await Context.SaveChangesAsync();
        return NotFound("Portfolio with id " + id + " does not exist!");
    }

    [HttpPost("BuyStock")]
    public async Task<ActionResult> BuyStock([FromBody] BuyStockModel bsm)
    {
        var stock = await Context.Stocks.FindAsync(bsm.StockId);
        if(stock == null)
            return NotFound("Stock not found");

        //var portfolio = await Context.Portfolios.FindAsync(bsm.PortfolioId);
        var portfolio = await Context.Portfolios
            .Include(p => p.BoughtStocks)
            .FirstOrDefaultAsync(p => p.ID == bsm.PortfolioId);
        if(portfolio == null)
            return NotFound("Portfolio not found");

        double buyCost = bsm.Quantity * stock.CurrentPrice;
        if(buyCost > portfolio.BankBalance)
            return BadRequest("Insufficient funds");

        var boughtStock = new BoughtStock
        {
            Stock = stock,
            BuyingPrice = stock.CurrentPrice,
            Quantity = bsm.Quantity
        };

        portfolio.BankBalance -= buyCost;

        if (portfolio.BoughtStocks == null)
            portfolio.BoughtStocks = new List<BoughtStock>();

        portfolio.BoughtStocks?.Add(boughtStock);

        var ret = new {
            portfolio,
            portfolio.BoughtStocks
        };

        await Context.SaveChangesAsync();

        return Ok(ret);
    }


    [HttpPost("SellStock")]
    public async Task<ActionResult> SellStock([FromBody] SellStockModel ssm)
    {
        // var stock = await Context.Stocks.FindAsync(ssm.StockId);
        // if (stock == null)
        //     return NotFound("Stock not found");

        var portfolio = await Context.Portfolios
            .Include(p => p.BoughtStocks)
            .ThenInclude(bs => bs.Stock)
            .FirstOrDefaultAsync(p => p.ID == ssm.PortfolioId);

        if (portfolio == null)
            return NotFound("Portfolio not found");

        var soldStock = portfolio.BoughtStocks?.FirstOrDefault(bs => bs.ID == ssm.BoughtStockId);
        if (soldStock == null || soldStock.Quantity < ssm.Quantity)
            return BadRequest("Invalid sell operation");

        double sellValue = ssm.Quantity /** stock.CurrentPrice*/ * soldStock.Stock.CurrentPrice;
        
        portfolio.BankBalance += sellValue;
        soldStock.Quantity -= ssm.Quantity;

        if (soldStock.Quantity == 0)
            portfolio.BoughtStocks?.Remove(soldStock);

        var ret = new
        {
            portfolio,
            portfolio.BoughtStocks
        };

        await Context.SaveChangesAsync();

        return Ok(ret);
    }

}
