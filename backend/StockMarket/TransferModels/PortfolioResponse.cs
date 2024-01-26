using Models;

namespace StockMarket.TransferModels
{
    public class PortfolioResponse
    {
        public required Portfolio portfolio { get; set; }
        public List<BoughtStock>? boughtStocks { get; set; }
    }
}
