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

    //[Test]
    //public async Task UpdateStockPageTest()
    //{
    //    int stockId = 2;
    //    await page.GotoAsync($"http://localhost:4200/update-stock/{stockId}");
    //    await Page.WaitForSelectorAsync(".update-stock-container");
    //    Assert.IsNotNull(await Page.QuerySelectorAsync(".update-stock-container"), "Update Stock page is displayed.");

    //    await page.ScreenshotAsync(new() { Path = "../../../Slike/updateStockPageTest.png" });
    //}

    [Test]
    public async Task UpdateStockPageTest()
    {
        //int stockId = 2;
        await page.GotoAsync($"http://localhost:4200/update-stock/15");

        await page.WaitForSelectorAsync("[data-testid='update-stock-container']");
        var updateStockPage = await page.QuerySelectorAsync("[data-testid='update-stock-container']");

        Assert.IsNotNull(updateStockPage, "Update Stock page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/updateStockPageTest.png" });
    }

    [Test]
    public async Task UpdateStockFormSubmissionTest()
    {
        await page.GotoAsync("http://localhost:4200/update-stock/36");

        await page.WaitForSelectorAsync("[data-testid='update-stock-container']");

        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='update-stock-container']"), "Update Stock page is displayed.");

        await page.FillAsync("[data-testid='symbol-input']", "AAPL");
        await page.FillAsync("[data-testid='company-input']", "Apple Inc.");
        await page.FillAsync("[data-testid='currentPrice-input']", "1500");

        await page.SetInputFilesAsync("[data-testid='logoUrl-input']", "C:\\Users\\Lenovo\\Desktop\\Projekat Testiranje 2\\logos\\AAPL.png");

        await page.WaitForSelectorAsync("[data-testid='update-stock-button']");
        await page.ClickAsync("[data-testid='update-stock-button']");

        await page.WaitForSelectorAsync("[data-testid='stocks-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='stocks-container']"), "Stocks page is displayed after form submission.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/updateStockFormSubmissionTest.png" });
    }

    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
