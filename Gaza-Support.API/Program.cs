using DataAccess.Extensions;
using Gaza_Support.API;
using Gaza_Support.API.Seeding;
using Gaza_Support.API.Settings;
using Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));

builder.Services
    .AddPresistance()
    .AddInfrastructure()
    .ServiceInjection(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DataSeeding.Invoke(app.Services.CreateScope().ServiceProvider).Wait();

app.Run();
