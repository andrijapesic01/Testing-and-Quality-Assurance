namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class StocksPageTests : PageTest
{
    IPage page;
    IBrowser browser;

    [SetUp]
    public async Task Setup()
    {
        browser = await Playwright.Chromium.LaunchAsync(new()
        {
            Headless = false,
            SlowMo = 2000
        });

        page = await browser.NewPageAsync(new()
        {
            ViewportSize = new()
            {
                Width = 1280,
                Height = 720
            },
            ScreenSize = new()
            {
                Width = 1280,
                Height = 720
            },
            RecordVideoSize = new()
            {
                Width = 1280,
                Height = 720
            },
            RecordVideoDir = "../../../Videos"
        });
    }

    [Test]
    public async Task StocksPageTest()
    {
        await page.GotoAsync("http://localhost:4200/stocks");

        await Page.WaitForSelectorAsync(".stocks-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".stocks-container"), "Stocks page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/stocksPage.png" });
    }

    [Test]
    public async Task UpdateStockBtnClickTest()
    {
        await page.GotoAsync("http://localhost:4200/stocks");

        await Page.WaitForSelectorAsync(".stock-item:first-child button[color='primary']");
        var updateButton = await Page.QuerySelectorAsync(".stock-item:first-child button[color='primary']");

        await updateButton.ClickAsync();

        await Page.WaitForSelectorAsync("app-update-stock");
        var updateStockPage = await Page.QuerySelectorAsync("app-update-stock");

        Assert.IsNotNull(updateStockPage, "Update Stock page is displayed.");
        await page.ScreenshotAsync(new() { Path = "../../../Slike/updateStockPageClicked.png" });
    }

    [Test]
    public async Task DeleteStockBtnClickTest()
    {
        await page.GotoAsync("http://localhost:4200/stocks");

        await Page.WaitForSelectorAsync(".stock-item:first-child button[color='warn']");
        var deleteButton = await Page.QuerySelectorAsync(".stock-item:first-child button[color='warn']");

        await deleteButton.ClickAsync();
        await page.ScreenshotAsync(new() { Path = "../../../Slike/deletStockPageClicked.png" });
    }

    [Test]
    public async Task BuyStockTest()
    {
        await page.GotoAsync("http://localhost:4200/stocks");

        await Page.WaitForSelectorAsync(".stock-item:first-child button[color='accent']");
        var buyButton = await Page.QuerySelectorAsync(".stock-item:first-child button[color='accent']");

        await buyButton.ClickAsync();

        await Page.WaitForSelectorAsync(".portfolio-selection");
        await Page.WaitForSelectorAsync(".quantity-input");

        await page.SelectOptionAsync(".portfolio-selection", new[] { "1" });

        await page.ScreenshotAsync(new() { Path = "../../../Slike/deletStockPageClicked.png" });
        
        await page.TypeAsync(".quantity-input input", "1");

        await page.ClickAsync(".stock-item:first-child button[color='accent']");
    }

    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
