using CommonAPI.Middleware;
using CommonAPIBusinessLayer.Configuration;
using CommonAPIBusinessLayer.Services.Clients;
using CommonAPIBusinessLayer.Services.Impl;
using CommonAPIBusinessLayer.Services.Interface;
using CommonAPICommon;
using CommonAPIDAL.AlfaVisionWebModels;
using CommonAPIDAL.VisionAppModels;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddScoped<SystemConfigurationManager>();

builder.Services.Configure<ISOConfig>(builder.Configuration.GetSection("ISOSettings"));
builder.Services.AddSingleton<ISOServiceConfig>();

// Register HttpClient for ISOServiceClient
builder.Services.AddHttpClient<ISOServiceClient>();

// Register ISOService
builder.Services.AddScoped<IISOService, ISOService>();


builder.Services.AddCors(opt =>
{
    opt.AddPolicy("TrexisCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<VisionAppEntities>(
                   options => options.UseSqlServer(builder.Configuration.GetConnectionString("VisionApp")));
builder.Services.AddDbContext<AlfaVisionWebEntities>(
          options => options.UseSqlServer(builder.Configuration.GetConnectionString("AlfaVisionWeb")));

// Define health checks
builder.Services.AddHealthChecks()
    .AddDiskStorageHealthCheck(s => s.AddDrive("C:\\", 1024), "Disk Check", Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded)
    .AddSqlServer(builder.Configuration.GetValue<string>("NLogConnectionString"), name: "Logging Database")
    .AddSqlServer(builder.Configuration.GetConnectionString("AlfaVisionWeb"), name: "AlfaVisionWeb Database")
    .AddSqlServer(builder.Configuration.GetConnectionString("VisionApp"), name: "VisionApp Database");

var app = builder.Build();

// Health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCors("TrexisCorsPolicy");
app.UseMiddleware<ApiLoggerMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

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