using CsvHelper.Configuration;
using CsvHelper;
using ETLWorkcerSevice.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ETLWorkcerSevice
{
    internal class ETLWorkerService : BackgroundService
    {
        private readonly ILogger<ETLWorkerService> _logger;
        public readonly ProductdbContext _dbContex;
        public readonly FileSettings _fileSettings;
        public readonly IServiceScopeFactory _scope;


        public ETLWorkerService(ILogger<ETLWorkerService> logger,
            IOptions<FileSettings> settings,
            IServiceScopeFactory scope) //DI
            //IOptions<FileSettings> settings) // DI
        {
            _logger = logger;
            _scope = scope;
            _fileSettings = settings.Value;
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
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async void Process()
        {
            //var fileSetting = _scope.CreateScope().ServiceProvider.GetRequiredService<FileSettings>();
            using (var scope = _scope.CreateScope())
            {
                //var fileSetting = scope.ServiceProvider.GetRequiredService<FileSettings>();
                var db = scope.ServiceProvider.GetRequiredService<ProductdbContext>();
                var files = Directory.GetFiles(_fileSettings.CsvFolder, "*.csv");
                foreach (var file in files)
                {
                    //Console.WriteLine("File: " + file);
                    try
                    {
                        Console.WriteLine(file);
                        //baca file csv
                        using (var reader = new StreamReader(file))
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            MissingFieldFound = null,
                            HeaderValidated = null,
                            HasHeaderRecord = true,
                        }))
                        {
                            var records = csv.GetRecords<Product2>();

                            // simpan ke database
                            await db.Product2s.AddRangeAsync(records);
                            await db.SaveChangesAsync();
                        }
                    }
                        // hapus file
                        //Console.WriteLine("File Deleted: " + file);
                        //File.Delete(file);
                    //}
                    catch (Exception ex)
                    {
                        throw new ArgumentException(ex.Message);
                    }
                    File.Delete(file);
                }

                //hapus
                //File.Delete(file);
            }

        }
        //private async void ProcessFile(string file, ProductdbContext db) 
        //{

        //    // baca file
        //    // simpan ke database
        //    Console.WriteLine("File created: " + file);
        //    // baca file csv     
        //    using (var reader = new StreamReader(file))
        //    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        //    {
        //        MissingFieldFound = null,
        //        HeaderValidated = null,
        //        HasHeaderRecord = true,
        //    }))
        //    {
        //        var records = csv.GetRecords<Product2>();

        //        // simpan ke database
        //        await db.Product2s.AddRangeAsync(records);
        //        await db.SaveChangesAsync();
        //    }
        //    //baca file
        //    //try
        //    //{
        //    //    Console.WriteLine("File Created: " + file);
        //    //    //baca file csv
        //    //    using (var reader = new StreamReader(file))
        //    //    {
        //    //        var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        //    //        {
        //    //            MissingFieldFound = null,
        //    //            HeaderValidated = null,
        //    //            HasHeaderRecord = true,
        //    //        });
        //    //        var records = csv.GetRecords<Product2>();

        //    //        // simpan ke database
        //    //        await db.Product2s.AddRangeAsync(records);
        //    //        await db.SaveChangesAsync();
        //    //    }
        //    //    // hapus file
        //    //    Console.WriteLine("File Deleted: " + file);
        //    //    File.Delete(file);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw new ArgumentException(ex.Message);
        //    //}
        //}
    }
}
