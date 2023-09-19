using Inventory.Entities;
using Inventory.Services.Product;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddSwaggerGen();
var InventoryDatabaseCS = builder.Configuration.GetConnectionString("InventoryDatabase");
builder.Services.AddDbContext<InventoryContext>(options =>
{
    options.UseSqlServer(InventoryDatabaseCS);
});
builder.Services.AddGrpc();
var app = builder.Build();
app.MapGrpcService<Inventory.GRPCServices.ProductService>();
// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
