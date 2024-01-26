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

        await Page.WaitForSelectorAsync(".add-portfolio-container form");

        Assert.IsNotNull(await Page.QuerySelectorAsync(".add-portfolio-container"), "Add Portfolio page is displayed.");

        await page.FillAsync("#ownerName", "John Doe");
        await page.FillAsync("#bankName", "Sample Bank");
        await page.FillAsync("#bankBalance", "10000");
        await page.FillAsync("#riskTolerance", "5");
        await page.FillAsync("#investmentStrategy", "Aggressive Growth");
        await page.ClickAsync(".add-portfolio-container form button[type='submit']");

        //await Page.WaitForSelectorAsync(".success-message");

        //Assert.IsNotNull(await Page.QuerySelectorAsync(".success-message"), "Form submitted successfully.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/addPortfolioPage.png" });
    }


    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
