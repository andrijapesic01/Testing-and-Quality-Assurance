
using System.Runtime.Intrinsics.X86;

namespace PlaywrightTests
{
    [TestFixture]
    internal class StockControllerTest : PlaywrightTest
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
        [TestCase("TSLA", "Tesla Inc.", "/images/e21ac457-5296-4895-bbd7-1a63306fe046.png", 1120)]
        public async Task AddStockTest(string symbol, string company, string logoURL, double currentPrice)
        {

            await using var response = await Request.PostAsync("/Stock/AddStock", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Symbol = symbol,
                    Company = company,
                    CurrentPrice = currentPrice,
                    LogoURL = logoURL
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("symbol").ToString(), Is.EqualTo(symbol));
                Assert.That(jsonResponse?.GetProperty("company").ToString(), Is.EqualTo(company));
                Assert.That(jsonResponse?.GetProperty("currentPrice").ToString(), Is.EqualTo(currentPrice.ToString()));
                Assert.That(jsonResponse?.GetProperty("logoURL").ToString(), Is.EqualTo(logoURL));
            });
        }

        [Test]
        public async Task GetAllStocksTest()
        {
            await using var response = await Request.GetAsync("/Stock/GetAllStocks");

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
        }

        [Test]
        [TestCase(14)]
        public async Task GetStockByIdTest(int stockID)
        {
            await using var response = await Request.GetAsync($"/Stock/GetStock/{stockID}");

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
            Assert.That(jsonResponse?.GetProperty("id").GetInt32(), Is.EqualTo(stockID));
        }

        [Test]
        [TestCase(14, "TSLA", "Tesla Inc.", "/images/e21ac457-5296-4895-bbd7-1a63306fe046.png", 1120)]
        public async Task UpdateStockTest(int stockID, string symbol, string company, string logoURL, double currentPrice)
        {
            await using var response = await Request.PutAsync($"/Stock/UpdateStock/{stockID}", new APIRequestContextOptions
            {
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                },
                DataObject = new
                {
                    Symbol = symbol,
                    Company = company,
                    CurrentPrice = currentPrice,
                    LogoURL = logoURL
                }
            });

            Assert.That(response.Status, Is.EqualTo(200));
            var jsonResponse = await response.JsonAsync();
            Assert.That(jsonResponse, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse?.GetProperty("symbol").ToString(), Is.EqualTo(symbol));
                Assert.That(jsonResponse?.GetProperty("company").ToString(), Is.EqualTo(company));
                Assert.That(jsonResponse?.GetProperty("logoURL").ToString(), Is.EqualTo(logoURL));
                Assert.That(jsonResponse?.GetProperty("currentPrice").ToString(), Is.EqualTo(currentPrice.ToString()));
            });
        }

        [Test]
        [TestCase(31)]
        public async Task DeleteStockTest(int stockId)
        {
            await using var response = await Request.DeleteAsync($"/Stock/DeleteStock/{stockId}");

            Assert.That(response.Status, Is.EqualTo(200));
            var textResponse = (await response.TextAsync()).Trim('"');

            Assert.That(textResponse, Is.EqualTo($"Stock with id {stockId} successfully deleted!"));
        }

        [TearDown]
        public async Task TearDownAPITesting()
        {
            await Request.DisposeAsync();
        }

    }
}
