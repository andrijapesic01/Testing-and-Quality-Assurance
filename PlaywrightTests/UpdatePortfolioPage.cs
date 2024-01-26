namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UpdatePorfolioPageTests : PageTest
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
    public async Task UpdatePortfolioPageTest()
    {
        int portfolioId = 5;

        await page.GotoAsync($"http://localhost:4200/update-portfolio/{portfolioId}");
        await Page.WaitForSelectorAsync(".add-portfolio-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".add-portfolio-container"), "Update Portfolio page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/updatePortfolioPageTest.png" });
    }

    [Test]
    public async Task UpdatePortfolioFormSubmissionTest()
    {
        int portfolioId = 5;

        await page.GotoAsync($"http://localhost:4200/update-portfolio/{portfolioId}");
        await Page.WaitForSelectorAsync(".center-button");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".center-button"), "Update Portfolio form is displayed.");

        await page.TypeAsync("#ownerName", "New Owner");
        await page.TypeAsync("#bankName", "New Bank");
        await page.TypeAsync("#bankBalance", "10000");
        await page.TypeAsync("#riskTolerance", "5");
        await page.TypeAsync("#investmentStrategy", "Conservative");

        await page.ClickAsync(".center-button");

        await Page.WaitForSelectorAsync(".portfolio-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".portfolio-container"), "View Portfolio page is displayed after form submission.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/updatePortfolioFormSubmissionTest.png" });
    }


    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
