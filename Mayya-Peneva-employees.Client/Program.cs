using Mayya_Peneva_employees.Client.Core.Helpers.Converters;
using Mayya_Peneva_employees.Client.Core.Helpers.Parsers;
using Mayya_Peneva_employees.Client.Core.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorBootstrap();

builder.Services.AddScoped<IDateParser, DateParser>();
builder.Services.AddScoped<IEmployeeConverter, EmployeeConverter>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Information));

await builder.Build().RunAsync();
