using ETLWorkcerSevice;
using ETLWorkcerSevice.Models;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// baca configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// baca connection string
var connectionString = configuration.GetConnectionString("MyDB");

// DI dependency Injection
builder.Services.AddDbContext<ProductdbContext>(options =>
            options.UseSqlServer(connectionString));
//var service = new ServiceCollection()
//    .AddDbContext<ProductdbContext>(options => options.UseSqlServer(connectionString))
//    .BuildServiceProvider();

// service biasanya setelah databse
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<ETLWorkcerSevice.ETLWorkerService>();

// DI File Settings
builder.Services.Configure<FileSettings>(configuration.GetSection("FileSettings"));

Console.WriteLine("Database Connect");

var host = builder.Build();
host.Run();