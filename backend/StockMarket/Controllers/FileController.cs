using Microsoft.AspNetCore.Http.HttpResults;

namespace StockMarket.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    public StockMarketContext Context { get; set; }
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileController(StockMarketContext context, IWebHostEnvironment webHostEnvironment)
    {
        Context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpPost("UploadImage")]
    public async Task<ActionResult<string>> UploadImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file");
        }

        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        string webRootPath = _webHostEnvironment.WebRootPath;

        string directoryPath = Path.Combine(webRootPath, "images");

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string filePath = Path.Combine(directoryPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        string path = $"/images/{fileName}";
        return Ok(path);
    }
}