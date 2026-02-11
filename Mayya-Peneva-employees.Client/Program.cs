using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorBootstrap();

builder.Services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Information));

await builder.Build().RunAsync();
