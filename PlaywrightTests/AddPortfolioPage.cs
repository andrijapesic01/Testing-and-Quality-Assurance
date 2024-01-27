namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AddPorfolioPageTests : PageTest
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
    public async Task AddPortfolioPageTest()
    {
        await page.GotoAsync("http://localhost:4200/add-portfolio");

        await page.WaitForSelectorAsync("[data-testid='add-portfolio-container'] form");

        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='add-portfolio-container']"), "Add Portfolio page is displayed.");

        await page.FillAsync("[data-testid='ownerName-input']", "Jelisaveta Kostanovic");
        await page.FillAsync("[data-testid='bankName-input']", "UniCredit");
        await page.FillAsync("[data-testid='bankBalance-input']", "12345");
        await page.FillAsync("[data-testid='riskTolerance-input']", "15");
        await page.FillAsync("[data-testid='investmentStrategy-input']", "Aggressive Growth");

        await page.WaitForSelectorAsync("[data-testid='add-portfolio-button']");
        await page.ClickAsync("[data-testid='add-portfolio-button']");

        await page.WaitForSelectorAsync("[data-testid='portfolios-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='portfolios-container']"), "View portfolios page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/addPortfolioPage.png" });
    }

    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
