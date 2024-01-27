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

        await page.GotoAsync($"http://localhost:4200/update-portfolio/5");
        await page.WaitForSelectorAsync("[data-testid='update-portfolio-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='update-portfolio-container']"), "Update Portfolio page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/updatePortfolioPageTest.png" });
    }

    [Test]
    public async Task UpdatePortfolioFormSubmissionTest()
    {
        await page.GotoAsync($"http://localhost:4200/update-portfolio/5");
        await page.WaitForSelectorAsync("[data-testid='update-portfolio-button']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='update-portfolio-button']"), "Update Portfolio form is displayed.");

        await page.FillAsync("[data-testid='ownerName-input']", "Vlastimir Ugljesic");
        await page.FillAsync("[data-testid='bankName-input']", "AIK Banka");
        await page.FillAsync("[data-testid='bankBalance-input']", "10892");
        await page.FillAsync("[data-testid='riskTolerance-input']", "25");
        await page.FillAsync("[data-testid='investmentStrategy-input']", "Conservative");

        await page.ClickAsync("[data-testid='update-portfolio-button']");

        await page.WaitForSelectorAsync("[data-testid='portfolio-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='portfolio-container']"), "View Portfolio page is displayed after form submission.");
        
        await page.ScreenshotAsync(new() { Path = "../../../Slike/updatePortfolioFormSubmissionTest.png" });
    }


    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
