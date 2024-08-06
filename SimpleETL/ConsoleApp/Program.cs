// See https://aka.ms/new-console-template for more information
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleETL.Models;
using System.Globalization;

Console.WriteLine("Simple ETL Application");

// baca configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// baca connection string
var connectionString = configuration.GetConnectionString("MyDB");

// DI dependency Injection

var service = new ServiceCollection()
    .AddDbContext<ProductdbContext>(options => options.UseSqlServer(connectionString))
    .BuildServiceProvider();

Console.WriteLine("Database Connect");

// baca folder csv
var csvFolder = configuration["CsvFolder"];

// monitor folder
FileSystemWatcher watcher = new FileSystemWatcher();
watcher.Path = csvFolder;
watcher.Filter = "*.csv";
watcher.Created += async (sender, eventArgs) =>
        await Watcher_Created(eventArgs.FullPath, service);
watcher.EnableRaisingEvents = true;

async Task Watcher_Created(string filepath, ServiceProvider service)
{
    try
    {
        Console.WriteLine("File Created: " + filepath);
        //baca file csv
        using (var reader = new StreamReader(filepath))
        {
            var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
                HasHeaderRecord = true,
            });
            var records = csv.GetRecords<Product2>();

            // simpan ke database
            var db = service.GetService<ProductdbContext>();
            await db.Product2s.AddRangeAsync(records);
            await db.SaveChangesAsync();
        }
        // hapus file
        File.Delete(filepath);
    }
    catch (Exception ex)
    {
        throw new ArgumentException(ex.Message);
    }
}
Console.WriteLine("Monitoring Folder" + csvFolder);
//Console.ReadLine();
while(true)
{
    await Task.Delay(1000);
}
