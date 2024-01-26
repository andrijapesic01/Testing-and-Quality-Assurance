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
        await Page.WaitForSelectorAsync(".portfolios-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".portfolios-container"), "Portfolios page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/portfoliosPageTest.png" });
    }

    [Test]
    public async Task ViewPortfolioTest()
    {
        await page.GotoAsync("http://localhost:4200/portfolios");
        await Page.WaitForSelectorAsync(".button-group");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".button-group"), "Button group is displayed.");

        await page.ClickAsync(".button-group button[color='primary']");

        await Page.WaitForSelectorAsync(".portfolio-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".portfolio-container"), "View portfolio page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/viewPortfolioTest.png" });
    }

    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
