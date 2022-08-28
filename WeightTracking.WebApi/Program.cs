using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WeightTracking.Application.Interfaces;
using WeightTracking.Application.Services;
using WeightTracking.DataAccess;
using WeightTracking.DataAccess.Entities;
using WeightTracking.DataAccess.Interfaces;
using WeightTracking.DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using WeightTracking.WebApi;
using Microsoft.OpenApi.Models;
using AutoMapper;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using WeightTracking.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .WriteTo.Console()
    .WriteTo.File("log.txt"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeightTracking API", Version = "v1" });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddAuthConfig(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("AppDbContext"), m =>
        m.MigrationsAssembly("WeightTracking.DataAccess")));

builder.Services.AddTransient<IPersonService, PersonService>();
builder.Services.AddTransient<IRecordService, RecordService>();

builder.Services.AddTransient<IGenericRepository<Person>, GenericRepository<Person>>();
builder.Services.AddTransient<IGenericRepository<Record>, GenericRepository<Record>>();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WeightTracking API V1");
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapGet("/", () => "Hello World!");

app.Run();

