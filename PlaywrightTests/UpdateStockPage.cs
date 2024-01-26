namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class UpdateStockPageTests : PageTest
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
    public async Task UpdateStockPageTest()
    {
        int stockId = 2;
        await page.GotoAsync($"http://localhost:4200/update-stock/{stockId}");
        await Page.WaitForSelectorAsync(".update-stock-container");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".update-stock-container"), "Update Stock page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/updateStockPageTest.png" });
    }

    [Test]
    public async Task UpdateStockFormSubmissionTest()
    {
        int stockId = 2;

        await page.GotoAsync($"http://localhost:4200/update-stock/{stockId}");
        await Page.WaitForSelectorAsync(".center-button");
        Assert.IsNotNull(await Page.QuerySelectorAsync(".center-button"), "Update Stock form is displayed.");

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


        await page.ScreenshotAsync(new() { Path = "../../../Slike/updateStockFormSubmissionTest.png" });
    }


    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
