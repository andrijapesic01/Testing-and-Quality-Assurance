namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class PortfolioPageTests : PageTest
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
    public async Task PortfolioPageTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolio/1");
        await Page.WaitForSelectorAsync(".portfolio-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".portfolio-container"), "Portfolio page is displayed.");
        await page.ScreenshotAsync(new() { Path = "../../../Slike/portfolioPage.png" });
    }

    [Test]
    public async Task SellStockTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolio/1");
        await Page.WaitForSelectorAsync(".bought-stocks");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".bought-stocks"), "Bought Stocks section is displayed.");

        await page.ClickAsync(".bought-stock button");
        await Page.WaitForSelectorAsync(".success-message");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".success-message"), "Stock sold successfully.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/sellStockTest.png" });
    }

    [Test]
    public async Task UpdatePortfolioTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolio/1");
        await Page.WaitForSelectorAsync(".button-group");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".button-group"), "Button group is displayed.");

        await page.ClickAsync(".button-group button[matColor='primary']");

        await Page.WaitForSelectorAsync(".update-portfolio-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".update-portfolio-container"), "Update portfolio page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/updatePortfolioTest.png" });
    }

    [Test]
    public async Task DeletePortfolioTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolio/1");
        await Page.WaitForSelectorAsync(".button-group");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".button-group"), "Button group is displayed.");

        await page.ClickAsync(".button-group button[matColor='warn']");

        await Page.WaitForSelectorAsync(".confirmation-dialog");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".confirmation-dialog"), "Delete confirmation dialog is displayed.");

        await page.ClickAsync(".confirmation-dialog button[matColor='warn']");

        await Page.WaitForSelectorAsync(".success-message");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".success-message"), "Portfolio deleted successfully.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/deletePortfolioTest.png" });
    }

    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
