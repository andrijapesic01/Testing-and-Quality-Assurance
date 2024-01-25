
using System.Runtime.Intrinsics.X86;

namespace PlaywrightTests
{
    [TestFixture]
    internal class PortfolioControllerTest : PlaywrightTest
    {
        private IAPIRequestContext Request;

        [SetUp]
        public async Task SetUpAPITesting()
        {
            var headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" }
            };

            Request = await Playwright.APIRequest.NewContextAsync(new()
            {
                BaseURL = "https://localhost:7193",
                ExtraHTTPHeaders = headers,
                IgnoreHTTPSErrors = true
            });
        }

        [Test]
        [TestCase("Ilija Petkovic", "UniCredit", 11510.00, 25, "Diversification")]
        public async Task AddPortfolioTest(string ownerName, string bankName, double bankBalance, int riskTolerance, string investmentStrategy)
        {
            await using var response = await Request.PostAsync("/Portfolio/AddPortfolio", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    OwnerName = ownerName,
                    BankName = bankName,
                    BankBalance = bankBalance,
                    RiskTolerance = riskTolerance, 
                    InvestmentStrategy = investmentStrategy
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));
            
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("ownerName").ToString(), Is.EqualTo(ownerName));
                Assert.That(jsonResponse?.GetProperty("bankName").ToString(), Is.EqualTo(bankName));
                Assert.That(jsonResponse?.GetProperty("bankBalance").ToString(), Is.EqualTo(bankBalance.ToString()));
                Assert.That(jsonResponse?.GetProperty("riskTolerance").ToString(), Is.EqualTo(riskTolerance.ToString()));
                Assert.That(jsonResponse?.GetProperty("investmentStrategy").ToString(), Is.EqualTo(investmentStrategy));
            });
        }

        [Test]
        public async Task GetAllPortfoliosTest()
        {
            await using var response = await Request.GetAsync("/Portfolio/GetAllPortfolios");

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
        }

        [Test]
        [TestCase(6)]
        public async Task GetPortfolioByIdTest(int portfolioID)
        {
            await using var response = await Request.GetAsync($"/Portfolio/GetPortfolio/{portfolioID}");

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            var portfolioObject = jsonResponse?.GetProperty("portfolio");
            Assert.That(portfolioObject?.GetProperty("id").GetInt32(), Is.EqualTo(portfolioID));
        }

        [Test]
        [TestCase(11, "Vladislav Krstic", "Intesa", 11511.00, 12, "Long-Term Growth")]
        public async Task UpdatePortfolioTest(int portfolioID, string ownerName, string bankName, double bankBalance, int riskTolerance, string investmentStrategy)
        {
            await using var response = await Request.PutAsync($"/Portfolio/UpdatePortfolio/{portfolioID}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    OwnerName = ownerName,
                    BankName = bankName,
                    BankBalance = bankBalance,
                    RiskTolerance = riskTolerance,
                    InvestmentStrategy = investmentStrategy
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));

            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("id").GetInt32(), Is.EqualTo(portfolioID));
                Assert.That(jsonResponse?.GetProperty("ownerName").ToString(), Is.EqualTo(ownerName));
                Assert.That(jsonResponse?.GetProperty("bankName").ToString(), Is.EqualTo(bankName));
                Assert.That(jsonResponse?.GetProperty("bankBalance").GetDouble(), Is.EqualTo(bankBalance));
                Assert.That(jsonResponse?.GetProperty("riskTolerance").GetInt32(), Is.EqualTo(riskTolerance));
                Assert.That(jsonResponse?.GetProperty("investmentStrategy").ToString(), Is.EqualTo(investmentStrategy));
            });
        }

        [Test]
        [TestCase(14)]
        public async Task DeletePortfolioTest(int portfolioID)
        {
            await using var response = await Request.DeleteAsync($"/Portfolio/DeletePortfolio/{portfolioID}");

            Assert.That(response.Status, Is.EqualTo(200));
            var textResponse = (await response.TextAsync()).Trim('"');

            Assert.That(textResponse, Is.EqualTo($"Portfolio with id {portfolioID} successfully deleted!"));
        }

        [Test]
        [TestCase(5, 4, 2)]
        public async Task BuyStockTest(int portfolioID, int stockID, int quantity)
        {
            await using var response = await Request.PostAsync($"/Portfolio/BuyStock", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    PortfolioId = portfolioID,
                    StockId = stockID,
                    Quantity = quantity
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
        }

        [Test]
        [TestCase(5, 16, 2)]
        public async Task SellStockTest(int portfolioID, int boughtStockID, int quantity)
        {
            await using var response = await Request.PostAsync($"/Portfolio/SellStock", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    PortfolioId = portfolioID,
                    BoughtStockId = boughtStockID,
                    Quantity = quantity
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
            
        }

        [TearDown]
        public async Task TearDownAPITesting()
        {
            await Request.DisposeAsync();
        }

    }
}
