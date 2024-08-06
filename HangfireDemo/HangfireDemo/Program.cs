using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using HangfireDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using HangfireDemo;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Hangfire.Dashboard;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//  config db
//var hangfireDb = builder.Configuration.GetConnectionString("HangfireDb");
var configdb = builder.Configuration.GetSection("ConnectionStrings:HangfireDB").Value;
builder.Services.AddDbContext<HangFireDbContext>(options =>
    options.UseSqlServer(configdb));

//hangfire service
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(configdb, new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    })
);

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

var options = new DashboardOptions()
{
    Authorization = new[] { new MyAuthorizationFilter() }
};
app.UseHangfireDashboard("/hangfire", options);



var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// API jobs

//app.MapGet("/fire-and-forget", () =>
//{
//    BackgroundJob.Enqueue(() =>
//            Console.WriteLine("Fire-and-forget job has been executed"));
//    return "Fire-and-forget job has beem scheduled";
//});

//app.MapGet("/delay-job", () =>
//{
//    BackgroundJob.Schedule(() =>
//            Console.WriteLine("Delay job has been executed"), TimeSpan.FromSeconds(5));
//    return "Delay job has beem scheduled";
//});

//app.MapGet("/recurring-job", () =>
//{
//    RecurringJob.AddOrUpdate("recurring-job", () =>
//            Console.WriteLine("Recurring job has been executed"), Cron.Minutely);
//    return "Recurring Job has beem scheduled";
//});


//pr ubah ke container ubah database larikan ke network yang sama
//app.MapPost("/insertdata-job", (Product products) =>
//{
//    BackgroundJob.Enqueue(() =>
//               Console.WriteLine("Fire-and-forget job has been executed"));
//    return "Fire-and-forget job has beem scheduled";

//    //baca data body json
//    //insert data ke database
//});

//app.MapPost("/insertdata-Fire-and-forget", async (Product products, HangFireDbContext db) =>
//{
//    try
//    {
//        db.Products.Add(products);
//        await db.SaveChangesAsync();
//        BackgroundJob.Enqueue(() => Console.WriteLine($"Produk {products.Code}-{products.Nama}-{products.Stock}-{products.Price} telah ditambahkan."));
//        var delay = TimeSpan.FromSeconds(5);
//        return Results.Created($"/products/{products.Id}", products);
//        //return Results.Ok(products);
//    }
//    catch (Exception ex)
//    {
//        // Handle the exception appropriately (e.g., log it or return an error response)
//        return Results.BadRequest(ex.Message);
//    }
//});

app.MapPost("/insertdata-recurring-job", async (Product products, HangFireDbContext db) =>
{
    try
    {
        db.Products.Add(products);
        await db.SaveChangesAsync();
        RecurringJob.AddOrUpdate("insertdata-job", () => Console.WriteLine($"produk {products.Code}-{products.Nama}-{products.Stock}-{products.Price} Berhasil Masuk"), Cron.Minutely);
        return Results.Created($"/products/{products.Id}", products);
        //return Results.Ok(products);
    }
    catch (Exception ex)
    {
        // Handle the exception appropriately (e.g., log it or return an error response)
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/insertdata-delay-job", async (Product products, HangFireDbContext db) =>
{
    try
    {
        db.Products.Add(products);
        await db.SaveChangesAsync();
        var delay = TimeSpan.FromSeconds(5);
        BackgroundJob.Schedule(() => Console.WriteLine($"produk {products.Code}-{products.Nama}-{products.Stock}-{products.Price} Berhasil Masuk"), delay);
        return Results.Created($"/products/{products.Id}", products);
        //return Results.Ok(products);
    }
    catch (Exception ex)
    {
        // Handle the exception appropriately (e.g., log it or return an error response)
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}