using ETLWorkerService;
using ETLWorkerService.Models;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// baca configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// baca connection string
var connectionString = configuration.GetConnectionString("MyDB");

// DI dependency injection
builder.Services.AddDbContext<ProductdbContext>(options => 
            options.UseSqlServer(connectionString));

//DI file setting
builder.Services.Configure<FileSettings>(configuration.GetSection("FileSettings"));

// services
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<ETLWorkerService.ETLWorkerService>();

var host = builder.Build();
host.Run();
