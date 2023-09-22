using Desafio.WebAPI.Configuration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(new CompactJsonFormatter(), "./logs/myapp.json")
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    builder.Services.AddIdentityConfig(builder.Configuration);

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddApiConfig();

    builder.Services.AddSwaggerConfig();

    builder.Services.ResolveDependencies();
    var app = builder.Build();

    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseApiConfig(app.Environment);

    app.UseSwaggerConfig(apiVersionDescriptionProvider);


    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }