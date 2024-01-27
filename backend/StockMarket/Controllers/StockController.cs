namespace StockMarket.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    public StockMarketContext Context { get; set; }

    public StockController(StockMarketContext context)
    {
        Context = context;
    }

    [HttpPost("AddStock")]
    public async Task<ActionResult<Stock>> AddStock([FromBody] StockModel sm)
    {
        if(string.IsNullOrEmpty(sm.Company) || string.IsNullOrEmpty(sm.Symbol) || sm.CurrentPrice < 0)
            return BadRequest("Invalid stock data");

        var stock = new Stock
        {
            Symbol = sm.Symbol,
            Company = sm.Company,
            CurrentPrice = sm.CurrentPrice,
            LogoURL = sm.LogoURL,
            PriceChange = 0,
            PercentChange = 0
        };

        Context.Stocks.Add(stock);

        await Context.SaveChangesAsync();
        return Ok(stock);
    }

    [HttpGet("GetStock/{id}")]
    public async Task<ActionResult<Stock>> GetStock(int id)
    {
        var stock = await Context.Stocks.FindAsync(id);

        if (stock == null)
            return NotFound("Stock with id " + id + " does not exist!");

        return Ok(stock);
    }

    [HttpGet("GetAllStocks")]
    public async Task<ActionResult<IEnumerable<Stock>>> GetAllStocks()
    {
        var stocks = await Context.Stocks.ToListAsync();
        return Ok(stocks);
    }

    [HttpPut("UpdateStock/{id}")]
    public async Task<ActionResult<Stock>> UpdateStock(int id, [FromBody] StockModel sm)
    {
        var stock = await Context.Stocks.FindAsync(id);
        if (stock == null)
        {
            return NotFound();
        }

        if (string.IsNullOrEmpty(sm.Company) || string.IsNullOrEmpty(sm.Symbol) || sm.CurrentPrice < 0)
            return BadRequest("Invalid stock data");


        stock.Symbol = sm.Symbol;
        stock.Company = sm.Company;

        stock.PriceChange = sm.CurrentPrice - stock.CurrentPrice;
        stock.PercentChange = (stock.PriceChange / stock.CurrentPrice) * 100;
        stock.CurrentPrice = sm.CurrentPrice;

        //Ne znam dal ce ovo treba
        if (sm.LogoURL != string.Empty && sm.LogoURL != "")
            stock.LogoURL = sm.LogoURL;

        await Context.SaveChangesAsync();

        return Ok(stock);
    }

    [HttpDelete("DeleteStock/{id}")]
    public async Task<ActionResult> DeleteStock(int id)
    {
        var stock = await Context.Stocks.FindAsync(id);
        if (stock == null)
            return NotFound("Stock with id " + id + " does not exist!");

        Context.Stocks.Remove(stock);
        await Context.SaveChangesAsync();

        return Ok("Stock with id " + id + " successfully deleted!");
    }

    
}
