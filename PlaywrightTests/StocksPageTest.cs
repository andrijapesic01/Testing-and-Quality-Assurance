using NUnit.Framework.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class StocksPageTests : PageTest
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
    public async Task StocksPageTest()
    {
        await page.GotoAsync("http://localhost:4200/stocks");

        await page.WaitForSelectorAsync("[data-testid='stocks-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='stocks-container']"), "Stocks page is displayed.");

        await page.ScreenshotAsync(new() { Path = "../../../Slike/stocksPage.png" });
    }

    [Test]
    public async Task UpdateStockBtnClickTest()
    {
        await page.GotoAsync("http://localhost:4200/stocks");

        await page.WaitForSelectorAsync("[data-testid='stock-item'] button[color='primary']");
        var updateButton = await page.QuerySelectorAsync("[data-testid='stock-item'] button[color='primary']");

        await updateButton.ClickAsync();

        await page.WaitForSelectorAsync("[data-testid='update-stock-container']");
        var updateStockPage = await page.QuerySelectorAsync("[data-testid='update-stock-container']");

        Assert.IsNotNull(updateStockPage, "Update Stock page is displayed.");
        await page.ScreenshotAsync(new() { Path = "../../../Slike/updateStockPageClicked.png" });
    }

    [Test]
    public async Task DeleteStockBtnClickTest()
    {
        await page.GotoAsync("http://localhost:4200/stocks");

        await page.WaitForSelectorAsync("[data-testid='stock-item'] button[color='warn']");
        var deleteButton = await page.QuerySelectorAsync("[data-testid='stock-item'] button[color='warn']");

        await deleteButton.ClickAsync();
        await page.ScreenshotAsync(new() { Path = "../../../Slike/deleteStockPageClicked.png" });
    }

    
    [Test]
    public async Task BuyStockTest()
    {
        await page.GotoAsync("http://localhost:4200/stocks");
        
        await page.ClickAsync("[data-testid='portfolio-selection'] mat-select");

        await page.WaitForSelectorAsync("[data-testid='portfolio-select']");
        await page.ClickAsync("[data-testid='portfolio-option']:first-child");

        await page.WaitForSelectorAsync("[data-testid='quantity-input']");
        await page.FillAsync("[data-testid='quantity-input'] input", "2");

        await page.ClickAsync("[data-testid='stock-item'] button[color='accent']");

        await page.WaitForSelectorAsync("[data-testid='portfolio-container']");
        Assert.IsNotNull(await page.QuerySelectorAsync("[data-testid='portfolio-container']"), "Portfolio page is displayed.");
    }



    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.DisposeAsync();
    }
}
