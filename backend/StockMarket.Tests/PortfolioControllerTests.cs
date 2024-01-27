
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StockMarket.TransferModels;


namespace StockMarket.Tests
{
    [TestFixture]
    public class PortfolioControllerTests
    {
        private PortfolioController _portfolioController;
        private StockMarketContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<StockMarketContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabasePortfolios_{Guid.NewGuid()}")
                .Options;

            _context = new StockMarketContext(options);

            var initialStocks = new List<Stock>
            {
                new Stock { Symbol = "AMZN", Company = "Amazon.com Inc.", CurrentPrice = 3000.0, LogoURL = "https://example.com/amazon-logo.png" },
                new Stock { Symbol = "NFLX", Company = "Netflix Inc.", CurrentPrice = 500.0, LogoURL = "https://example.com/netflix-logo.png" },
                new Stock { Symbol = "TSLA", Company = "Tesla Inc.", CurrentPrice = 700.0, LogoURL = "https://example.com/tesla-logo.png" },
            };

            _context.Stocks.AddRange(initialStocks);

            var initialData = new List<Portfolio>
            {
                new Portfolio { OwnerName = "Milojko Pantic", BankName = "Intesa", BankBalance = 11250.0, RiskTolerance = 10, InvestmentStrategy = "Long-Term Growth" },
                new Portfolio { OwnerName = "Radmila Kostadinovic", BankName = "UniCredit", BankBalance = 6789.55, RiskTolerance = 8, InvestmentStrategy = "Index Fund Investing" },
                new Portfolio { OwnerName = "Jelisaveta Petronijevic", BankName = "BNP Paribas", BankBalance = 56901.0, RiskTolerance = 25, InvestmentStrategy = "Diversification" },
            };

            //_context.Portfolios.AddRange(initialData);

            var boughtStocks = new List<BoughtStock>
            {
                new BoughtStock { Stock = initialStocks[0], BuyingPrice = 3000.0, Quantity = 10 },
                new BoughtStock { Stock = initialStocks[1], BuyingPrice = 500.0, Quantity = 5 },
            };

            initialData[0].BoughtStocks = boughtStocks;


            _context.Portfolios.AddRange(initialData);
            _context.SaveChanges();

            _portfolioController = new PortfolioController(_context);
        }

        [TestCase("Milos Petrovic", "Raiffeisen", 15000.0, 12, "Value Investing")]
        [TestCase("Ana Jovanovic", "Societe Generale", 7800.55, 15, "Growth Investing")]
        [TestCase("Marko Nikolic", "UniCredit", 45000.0, 20, "Index Fund Investing")]
        public async Task AddPortfolio_ValidPortfolio_ReturnsOkResult(string ownerName, string bankName, double bankBalance, int riskTolerance, string investmentStrategy)
        {
            var portfolioModel = new PortfolioModel
            {
                OwnerName = ownerName,
                BankName = bankName,
                BankBalance = bankBalance,
                RiskTolerance = riskTolerance,
                InvestmentStrategy = investmentStrategy
            };

            var actionResult = await _portfolioController.AddPortfolio(portfolioModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());

            var okResult = actionResult.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var addedPortfolio = okResult.Value as Portfolio;
            Assert.That(addedPortfolio, Is.Not.Null);

            Assert.That(ownerName, Is.EqualTo(addedPortfolio.OwnerName));
            Assert.That(bankName, Is.EqualTo(addedPortfolio.BankName));
            Assert.That(bankBalance, Is.EqualTo(addedPortfolio.BankBalance));
            Assert.That(riskTolerance, Is.EqualTo(addedPortfolio.RiskTolerance));
            Assert.That(investmentStrategy, Is.EqualTo(addedPortfolio.InvestmentStrategy));
        }

        [Test]
        public async Task AddPortfolio_MissingInformation_ReturnsBadRequest()
        {
            var invalidPortfolioModel = new PortfolioModel
            {
                BankBalance = 10000.0,
                RiskTolerance = 15,
                InvestmentStrategy = "Long-term investment"
            };

            var actionResult = await _portfolioController.AddPortfolio(invalidPortfolioModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());

            var badRequest = actionResult.Result as BadRequestObjectResult;
            Assert.That(badRequest, Is.Not.Null);

            var errorMessage = badRequest.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("Invalid portfolio information"));
        }

        [Test]
        public async Task AddPortfolio_InvalidBankBalanceInformation_ReturnsBadRequest()
        {
            var invalidPortfolioModel = new PortfolioModel
            {
                OwnerName = "Viktorija Nedeljkovic",
                BankName = "Intesa", 
                BankBalance = -1500.0,
                RiskTolerance = 15,
                InvestmentStrategy = "Long-term investment"
            };

            var actionResult = await _portfolioController.AddPortfolio(invalidPortfolioModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());

            var badRequest = actionResult.Result as BadRequestObjectResult;
            Assert.That(badRequest, Is.Not.Null);

            var errorMessage = badRequest.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("Invalid bank balance"));
        }

        [Test]
        public async Task GetAllPortfolios_ReturnsListOfPortfolios()
        {
            var result = await _portfolioController.GetAllPortfolios();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var portfolios = okResult.Value as List<Portfolio>;
            Assert.That(portfolios, Is.Not.Null.And.Not.Empty);
            Assert.That(portfolios.Count, Is.EqualTo(3));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GetPortfolio_ExistingPortfolioID_ReturnsPortfolio(int portfolioID)
        {
            var result = await _portfolioController.GetPortfolio(portfolioID);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

        }

        [TestCase(777)]
        [TestCase(888)]
        [TestCase(999)]
        public async Task GetPortfolio_NonExistingPortfolioId_ReturnsNotFound(int portfolioID)
        {
            var result = await _portfolioController.GetPortfolio(portfolioID);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);

            var errorMessage = notFoundResult.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("does not exist"));
        }

        [TestCase(1, "Milojko Pantic", "Banca Intesa", 15000.0, 12, "Value Investing")]
        [TestCase(2, "Radmila Kostadinovic", "UniCredit", 7800.55, 15, "Growth Investing")]
        [TestCase(3, "Jelisaveta Petronijevic", "BNP Paribas", 45000.0, 20, "Index Fund Investing")]
        public async Task UpdatePortfolio_ExistingPortfolio_ReturnsOkResult(int portfolioID, string ownerName, string newBankName, double newBalance, int newRiskTolerance, string newInvestmentStrategy)
        {
            var portfolioModel = new PortfolioModel
            {
                OwnerName = ownerName,
                BankName = newBankName,
                BankBalance = newBalance,
                RiskTolerance = newRiskTolerance,
                InvestmentStrategy = newInvestmentStrategy
            };

            var actionResult = await _portfolioController.UpdatePortfolio(portfolioID, portfolioModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<OkObjectResult>());

            var okResult = actionResult.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var updatedPortfolio = okResult.Value as Portfolio;
            Assert.That(updatedPortfolio, Is.Not.Null);

            Assert.That(portfolioID, Is.EqualTo(updatedPortfolio.ID));
            Assert.That(ownerName, Is.EqualTo(updatedPortfolio.OwnerName));
            Assert.That(newBankName, Is.EqualTo(updatedPortfolio.BankName));
            Assert.That(newBalance, Is.EqualTo(updatedPortfolio.BankBalance));
            Assert.That(newRiskTolerance, Is.EqualTo(updatedPortfolio.RiskTolerance));
            Assert.That(newInvestmentStrategy, Is.EqualTo(updatedPortfolio.InvestmentStrategy));
        }

        [TestCase(1000, "Milos Petrovic", "Raiffeisen", 18950.88, 15, "Growth Investing")]
        [TestCase(999, "Ana Jovanovic", "Societe Generale", 7800.55, 22, "Value Investing")]
        [TestCase(998, "Marko Nikolic", "UniCredit", 45000.0, 18, "Index Fund Investing")]
        public async Task UpdatePortfolio_NonExistingPortfolio_ReturnsNotFound(int portfolioID, string ownerName, string newBankName, double newBalance, int newRiskTolerance, string newInvestmentStrategy)
        {
            var portfolioModel = new PortfolioModel
            {
                OwnerName = ownerName,
                BankName = newBankName,
                BankBalance = newBalance,
                RiskTolerance = newRiskTolerance,
                InvestmentStrategy = newInvestmentStrategy
            };

            var result = await _portfolioController.UpdatePortfolio(portfolioID, portfolioModel);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdatePortfolio_MissingInformation_ReturnsBadRequest()
        {
            var invalidPortfolioModel = new PortfolioModel
            {
                RiskTolerance = 15,
                InvestmentStrategy = "Long-term investment"
            };

            var actionResult = await _portfolioController.UpdatePortfolio(1, invalidPortfolioModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.Result, Is.TypeOf<BadRequestObjectResult>());

            var badRequest = actionResult.Result as BadRequestObjectResult;
            Assert.That(badRequest, Is.Not.Null);

            var errorMessage = badRequest.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("Invalid portfolio information"));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task DeletePortfolio_ExistingPortfolio_ReturnsOkResult(int portfolioID)
        {
            var result = await _portfolioController.DeletePortfolio(portfolioID);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<OkObjectResult>());

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var successMessage = okResult.Value as string;
            Assert.That(successMessage, Is.Not.Null.And.Contains("successfully deleted"));
        }

        [TestCase(111)]
        [TestCase(222)]
        [TestCase(333)]
        public async Task DeletePortfolio_NonExistingPortfolio_ReturnsNotFound(int portfolioID)
        {
            var result = await _portfolioController.DeletePortfolio(portfolioID);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);

            var errorMessage = notFoundResult.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("does not exist"));
        }

        [TestCase(1, 1, 1)]
        public async Task BuyStock_SuccessfulPurchase_ReturnsOkResult(int portfolioId, int stockId, int quantity)
        {
            var buyStockModel = new BuyStockModel
            {
                PortfolioId = portfolioId,
                StockId = stockId,
                Quantity = quantity
            };

            var actionResult = await _portfolioController.BuyStock(buyStockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

        }

        [TestCase(1, 1, 100)]
        public async Task BuyStock_InsufficientFunds_ReturnsBadRequest(int portfolioId, int stockId, int quantity)
        {
            var buyStockModel = new BuyStockModel
            {
                PortfolioId = portfolioId,
                StockId = stockId,
                Quantity = quantity
            };

            var actionResult = await _portfolioController.BuyStock(buyStockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());

            var badRequest = actionResult as BadRequestObjectResult;
            Assert.That(badRequest, Is.Not.Null);

            var errorMessage = badRequest.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("Insufficient funds"));
        }

        [TestCase(1, 999, 5)]
        public async Task BuyStock_NonExistingStock_ReturnsNotFound(int portfolioId, int stockId, int quantity)
        {
            var buyStockModel = new BuyStockModel
            {
                PortfolioId = portfolioId,
                StockId = stockId,
                Quantity = quantity
            };

            var actionResult = await _portfolioController.BuyStock(buyStockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = actionResult as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);

            var errorMessage = notFoundResult.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("Stock not found"));
        }

        [TestCase(999, 1, 5)]
        public async Task BuyStock_NonExistingPortfolio_ReturnsNotFound(int portfolioId, int stockId, int quantity)
        {
            var buyStockModel = new BuyStockModel
            {
                PortfolioId = portfolioId,
                StockId = stockId,
                Quantity = quantity
            };

            var actionResult = await _portfolioController.BuyStock(buyStockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());

            var notFoundResult = actionResult as NotFoundObjectResult;
            Assert.That(notFoundResult, Is.Not.Null);

            var errorMessage = notFoundResult.Value as string;
            Assert.That(errorMessage, Is.Not.Null.And.Contains("Portfolio not found"));
        }

        [TestCase(1, 1, 5)]
        public async Task SellStock_SuccessfulSale_ReturnsOkResult(int portfolioId, int boughtStockId, int quantity)
        {
            var sellStockModel = new SellStockModel
            {
                PortfolioId = portfolioId,
                BoughtStockId = boughtStockId,
                Quantity = quantity
            };

            var actionResult = await _portfolioController.SellStock(sellStockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
        }

        [TestCase(1, 1, 100)]
        public async Task SellStock_InvalidSale_ReturnsBadRequest(int portfolioId, int boughtStockId, int quantity)
        {
            var sellStockModel = new SellStockModel
            {
                PortfolioId = portfolioId,
                BoughtStockId = boughtStockId,
                Quantity = quantity
            };

            var actionResult = await _portfolioController.SellStock(sellStockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [TestCase(1, 999, 5)]
        public async Task SellStock_NonExistingStock_ReturnsBadRequest(int portfolioId, int boughtStockId, int quantity)
        {
            var sellStockModel = new SellStockModel
            {
                PortfolioId = portfolioId,
                BoughtStockId = boughtStockId,
                Quantity = quantity
            };

            var actionResult = await _portfolioController.SellStock(sellStockModel);

            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
