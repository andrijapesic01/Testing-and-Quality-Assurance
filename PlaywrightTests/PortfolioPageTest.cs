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
        await page.GotoAsync("http://localhost:4200/portfolio/5");
        await page.WaitForSelectorAsync("[data-testid='portfolio-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='portfolio-container']"), "Portfolio page is displayed.");
        await page.ScreenshotAsync(new() { Path = "../../../Slike/portfolioPage.png" });
    }

    [Test]
    public async Task SellStockTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolio/9");
        
        await page.WaitForSelectorAsync("[data-testid='bought-stock'] button[color='warn']");
        var sellButton = await page.QuerySelectorAsync("[data-testid='bought-stock'] button[color='warn']");

        await sellButton.ClickAsync();

        await page.WaitForSelectorAsync("[data-testid='portfolio-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='portfolio-container']"), "View portfolio page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/sellStockTest.png" });
    }

    [Test]
    public async Task UpdatePortfolioTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolio/5");
        await page.WaitForSelectorAsync("[data-testid='button-group']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='button-group']"), "Button group is displayed.");

        await page.ClickAsync("[data-testid='button-group'] button[matColor='primary']");

        await page.WaitForSelectorAsync("[data-testid='update-portfolio-container']");

        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='update-portfolio-container']"), "Update portfolio page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/updatePortfolioTest.png" });
    }

    [Test]
    public async Task DeletePortfolioTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolio/11");
        await page.WaitForSelectorAsync("[data-testid='button-group']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='button-group']"), "Button group is displayed.");

        await page.ClickAsync("[data-testid='button-group'] button[matColor='warn']");

        await page.WaitForSelectorAsync("[data-testid='portfolios-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='portfolios-container']"), "View portfolios page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/deletePortfolioTest.png" });
    }

    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
