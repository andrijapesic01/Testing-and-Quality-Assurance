namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class PortfoliosPageTests : PageTest
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
    public async Task PortfoliosPageTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolios");
        await page.WaitForSelectorAsync("[data-testid='portfolios-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='portfolios-container']"), "Portfolios page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/portfoliosPageTest.png" });
    }

    [Test]
    public async Task ViewPortfolioTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolios");
        
        await page.WaitForSelectorAsync("[data-testid='portfolio-item'] button[color='primary']");
        var viewButton = await page.QuerySelectorAsync("[data-testid='portfolio-item'] button[color='primary']");

        await viewButton.ClickAsync();

        //added
        await page.WaitForSelectorAsync("[data-testid='portfolio-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='portfolio-container']"), "Portfolio page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/viewPortfolioTest.png" });
    }


    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
