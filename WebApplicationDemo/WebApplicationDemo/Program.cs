using Microsoft.EntityFrameworkCore;
//using WebApplicationDemo.Models;
using WebApplicationDemo.RapidModels;
using WebApplicationDemo.EF;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//db
//var configdbtest = builder.Configuration.GetSection("ConnectionStrings:TestDB").Value;
//builder.Services.AddDbContext<TestDBContext>(options =>
//    options.UseSqlServer(configdbtest));
var configdbrapid = builder.Configuration.GetSection("ConnectionStrings:RapidDb").Value;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configdbrapid));
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

builder.Services.AddScoped<ICategory, CategoryEF>();
builder.Services.AddScoped<IProduct, ProductEF>();
builder.Services.AddScoped<IOrderHeaders, OrderHeaderEF>();
builder.Services.AddScoped<IOrderDetail, OrderDetailsDAL>();
builder.Services.AddScoped<IWallet, WalletDAL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

//app.MapGet("/test", async (TestDBContext dbContext) =>
//{
//    return await dbContext.Tests.ToListAsync();
//});

app.MapGet("/kategori", async (AppDbContext dbContext) =>
{
    return await dbContext.Categories.ToListAsync();
});

app.UseAuthorization();

app.MapControllers();

app.Run();
