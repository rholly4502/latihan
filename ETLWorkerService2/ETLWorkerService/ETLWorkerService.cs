using CsvHelper.Configuration;
using CsvHelper;
using ETLWorkerService.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLWorkerService
{
    public class ETLWorkerService : BackgroundService
    {
        private readonly ILogger<ETLWorkerService> _logger;
        private readonly FileSettings _fileSettings;
        private readonly IServiceScopeFactory _scope;

        public ETLWorkerService(ILogger<ETLWorkerService> logger,
            IOptions<FileSettings> settings,
            IServiceScopeFactory scope) // DI
        {
            _logger = logger;
            _scope = scope;
            _fileSettings = settings.Value;
            // lazy load pattern
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("ETLWorkerService running at: {time}", DateTimeOffset.Now);
                }
                Process();
                await Task.Delay(2000, stoppingToken);
            }
        }
        private async void Process()
        {
            try
            {
                // baca files
                using (var scope = _scope.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ProductdbContext>();
                    var files = Directory.GetFiles(_fileSettings.CsvFolder, "*.csv");
                    foreach (var file in files)
                    {
                        Console.WriteLine("File: " + file);
                        //ProcessFile(file, db);
                        // baca file csv     
                        using (var reader = new StreamReader(file))
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            MissingFieldFound = null,
                            HeaderValidated = null,
                            HasHeaderRecord = true,
                        }))
                        {
                            var records = csv.GetRecords<Product>();

                            // simpan ke database
                            await db.Products.AddRangeAsync(records);
                            await db.SaveChangesAsync();
                        }
                        Console.WriteLine("Remove File: " + file);
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing files");
            }
                      
        }
        private async void ProcessFile(string file, ProductdbContext db)
        {
            // baca file
            // simpan ke database
            Console.WriteLine("File created: " + file);
            // baca file csv     
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
                HasHeaderRecord = true,
            }))
            {
                var records = csv.GetRecords<Product>();

                // simpan ke database
                await db.Products.AddRangeAsync(records);
                await db.SaveChangesAsync();
            }
        }
    }
}
