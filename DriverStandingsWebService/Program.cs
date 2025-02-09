using DriverStandingsWebService.BusinessLogic;
using DriverStandingsWebService.DataAccess;
using DriverStandingsWebService.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IDriverStandingsRepository, DriverStandingsRepository>();
builder.Services.AddScoped<IDriverStandingsBusinessLogic, DriverStandingsBusinessLogic>();
builder.Services.AddScoped<IDriverStandingsService, DriverStandingsService>();

builder.Services.AddControllers().AddXmlDataContractSerializerFormatters();
builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "A .NET 8 Web API supporting Swagger"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
