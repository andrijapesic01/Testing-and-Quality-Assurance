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

        await page.WaitForSelectorAsync("[data-testid='add-stock-container']");

        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='add-stock-container']"), "Add Stock page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/addStockPageTest.png" });
    }

    [Test]
    public async Task AddStockFormSubmissionTest()
    {
        await page.GotoAsync("http://localhost:4200/add-stock");

        await page.WaitForSelectorAsync("[data-testid='add-stock-container']");

        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='add-stock-container']"), "Add Stock page is displayed.");

        await page.TypeAsync("[data-testid='symbol-input']", "AAPL");
        await page.TypeAsync("[data-testid='company-input']", "Apple Inc.");
        await page.TypeAsync("[data-testid='currentPrice-input']", "1500");

        await page.SetInputFilesAsync("[data-testid='logoUrl-input']", "C:\\Users\\Lenovo\\Desktop\\Projekat Testiranje 2\\logos\\AAPL.png");

        await page.WaitForSelectorAsync("[data-testid='add-stock-button']");
        await page.ClickAsync("[data-testid='add-stock-button']");

        await page.WaitForSelectorAsync("[data-testid='stocks-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='stocks-container']"), "Stocks page is displayed after form submission.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/addStockPageTest.png" });
    }


    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
