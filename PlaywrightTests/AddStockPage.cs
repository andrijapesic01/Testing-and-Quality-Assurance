namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AddStockPageTests : PageTest
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
    public async Task AddStockPageTest()
    {
        await page.GotoAsync("http://localhost:4200/add-stock");
        await Page.WaitForSelectorAsync(".add-stock-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".add-stock-container"), "Add Stock page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/addStockPageTest.png" });
    }

    [Test]
    public async Task AddStockFormSubmissionTest()
    {
        await page.GotoAsync("http://localhost:4200/add-stock");
        await Page.WaitForSelectorAsync(".center-button");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".center-button"), "Add Stock form is displayed.");

        await page.TypeAsync("#symbol", "AAPL");
        await page.TypeAsync("#company", "Apple Inc.");
        await page.TypeAsync("#currentPrice", "1500");

        await page.SetInputFilesAsync("#logoUrl", new[]
        {
            new FilePayload
            {
                Name = "logo.png",
                MimeType = "image/png",
                Buffer = Convert.FromBase64String("base64-encoded-image-data")
            }
        });

        await page.ClickAsync(".center-button");

        await Page.WaitForSelectorAsync(".home-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".home-container"), "Home page is displayed after form submission.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/addStockFormSubmissionTest.png" });
    }


    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
