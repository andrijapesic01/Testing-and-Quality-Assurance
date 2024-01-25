using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StockMarketContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StockMarketCS"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins( "http://127.0.0.1:5500",
                            "https://127.0.0.1:5500",
                            "http://localhost:5500",
                            "https://localhost:5500",
                            "http://127.0.0.1:5555",
                            "https://127.0.0.1:5555",
                            "http://localhost:4200",
                            "https://localhost:4200");
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddSingleton<IWebHostEnvironment, WebHostEnvironment>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORS");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
