using Microsoft.AspNetCore.Mvc;

namespace StockMarket.Tests
{
    [TestFixture]
    public class StockControllerTests
    {
        private StockController _stockController;
        private StockMarketContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<StockMarketContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabaseStocks_{Guid.NewGuid()}")
                .Options;

            _context = new StockMarketContext(options);

            var initialData = new List<Stock>
            {
                new Stock { Symbol = "AMZN", Company = "Amazon.com Inc.", CurrentPrice = 3000.0, LogoURL = "https://example.com/amazon-logo.png" },
                new Stock { Symbol = "NFLX", Company = "Netflix Inc.", CurrentPrice = 500.0, LogoURL = "https://example.com/netflix-logo.png" },
                new Stock { Symbol = "TSLA", Company = "Tesla Inc.", CurrentPrice = 700.0, LogoURL = "https://example.com/tesla-logo.png" },
            };

            _context.Stocks.AddRange(initialData);
            _context.SaveChanges();

            _stockController = new StockController(_context);
        }

        [Test]
        [TestCase("AAPL", "Apple Inc.", 150.0, "https://example.com/apple-logo.png")]
        [TestCase("GOOGL", "Alphabet Inc.", 2000.0, "https://example.com/alphabet-logo.png")]
        [TestCase("MSFT", "Microsoft Corporation", 300.0, "https://example.com/alphabet-logo.png")]
        public async Task AddStock_ValidStock_ReturnsOkResult(string symbol, string company, double currentPrice, string logoURL)
        {
            var stockModel = new StockModel
            {
                Symbol = symbol,
                Company = company,
                CurrentPrice = currentPrice,
                LogoURL = logoURL
            };

            var actionResult = await _stockController.AddStock(stockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());

            var okResult = actionResult.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var addedStock = okResult.Value as Stock;
            Assert.That(addedStock, Is.Not.Null);

            Assert.That(symbol, Is.EqualTo(addedStock.Symbol));
            Assert.That(company, Is.EqualTo(addedStock.Company));
            Assert.That(currentPrice, Is.EqualTo(addedStock.CurrentPrice));
            Assert.That(logoURL, Is.EqualTo(addedStock.LogoURL));
        }

        [Test]
        public async Task AddStock_InvalidData_ReturnsBadRequest()
        {
            var stockModel = new StockModel
            {
                Symbol = "AMD",
                CurrentPrice = 555.5,
                LogoURL = "amd-logo.png"
            };

            var actionResult = await _stockController.AddStock(stockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());

            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);

            var errorMessage = badRequestResult.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("Invalid stock data"));
        }

        [Test]
        public async Task AddStock_NegativeStockPrice_ReturnsBadRequest()
        {
            var stockModel = new StockModel
            {
                Symbol = "AMD",
                Company = "Advanced Micro Devices Inc.",
                CurrentPrice = -555.5,
                LogoURL = "amd-logo.png"
            };

            var actionResult = await _stockController.AddStock(stockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());

            var badRequestResult = actionResult.Result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);

            var errorMessage = badRequestResult.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("Invalid stock data"));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GetStock_ExistingStockID_ReturnsStock(int stockID)
        {
            var result = await _stockController.GetStock(stockID);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var stock = okResult.Value as Stock;
            Assert.That(stock, Is.Not.Null);

            Assert.That(stockID, Is.EqualTo(stock.ID));
        }

        [Test]
        [TestCase(777)]
        [TestCase(888)]
        [TestCase(999)]
        public async Task GetStock_NonExistingStockId_ReturnsNotFound(int stockID)
        {
            var result = await _stockController.GetStock(stockID);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);

            var errorMessage = notFoundResult.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("does not exist"));
        }

        [Test]
        [TestCase(1, "AMZN", "Amazon.com Inc.", 2200.0, "https://example.com/amazon-logo.png")]
        [TestCase(2, "NFLX", "Netflix Inc.", 302.0, "https://example.com/new-netflix-logo.png")]
        [TestCase(3, "TSLA", "Tesla Inc.", 888.0, "https://example.com/old-tesla-logo.png")]
        public async Task UpdateStock_ExistingStock_ReturnsOkResult(int stockID, string symbol, string company, double newPrice, string newLogoURL)
        {
            var stockModel = new StockModel
            {
                Symbol = symbol,
                Company = company,
                CurrentPrice = newPrice,
                LogoURL = newLogoURL
            };

            var actionResult = await _stockController.UpdateStock(stockID, stockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());

            var okResult = actionResult.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var updatedStock = okResult.Value as Stock;
            Assert.That(updatedStock, Is.Not.Null);

            Assert.That(stockID, Is.EqualTo(updatedStock.ID));
            Assert.That(symbol, Is.EqualTo(updatedStock.Symbol));
            Assert.That(company, Is.EqualTo(updatedStock.Company));
            Assert.That(newPrice, Is.EqualTo(updatedStock.CurrentPrice));
            Assert.That(newLogoURL, Is.EqualTo(updatedStock.LogoURL));
        }

        [Test]
        [TestCase(1000, "AMZN", "Amazon.com Inc.", 1895.88, "https://example.com/amazon-logo.png")]
        [TestCase(999, "NFLX", "Netflix Inc.", 311.67, "https://example.com/new-netflix-logo.png")]
        [TestCase(998, "TSLA", "Tesla Inc.", 828.2, "https://example.com/old-tesla-logo.png")]
        public async Task UpdateStock_NonExistingStock_ReturnsNotFound(int stockID, string symbol, string company, double newPrice, string newLogoURL)
        {
            var stockModel = new StockModel
            {
                Symbol = symbol,
                Company = company,
                CurrentPrice = newPrice,
                LogoURL = newLogoURL
            };

            var result = await _stockController.UpdateStock(stockID, stockModel);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateStock_InvalidData_ReturnsBadRequest()
        {
            var stockModel = new StockModel
            {
                Symbol = "AMD",
                CurrentPrice = -3100.99,
            };

            var actionResult = await _stockController.UpdateStock(1, stockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task DeleteStock_ExistingStock_ReturnsOkResult(int stockID)
        {
            var result = await _stockController.DeleteStock(stockID);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var successMessage = okResult.Value as string;
            Assert.That(successMessage, Is.Not.Null.And.Contains("successfully deleted"));
        }

        [Test]
        [TestCase(111)]
        [TestCase(222)]
        [TestCase(333)]
        public async Task DeleteStock_NonExistingStock_ReturnsNotFound(int stockID)
        {
            var result = await _stockController.DeleteStock(stockID);
            Console.WriteLine($"Test result for stock ID {stockID}: {result}");


            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);

            var errorMessage = notFoundResult.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("does not exist"));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}