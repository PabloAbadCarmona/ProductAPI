using Serilog;
using FluentValidation.AspNetCore;
using ProductAPI.Interfaces.Repositories;
using ProductAPI.Repositories;
using ProductAPI.Interfaces.Handlers;
using ProductAPI.Handlers;
using ProductAPI.Services;
using ProductAPI.Interfaces.Services;
using FluentValidation;
using ProductAPI.Commands;
using ProductAPI.Validators;
using ProductAPI.Models;
using Polly.Extensions.Http;
using Polly;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/ProductApi.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddScoped<IValidator<CreateProductCommand>, CreateProductCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateProductCommand>, UpdateProductCommandValidator>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IProductHandler, ProductHandler>();


builder.Services.Configure<DiscountServiceSettings>(builder.Configuration.GetSection("DiscountServiceSettings"));
builder.Services.AddHttpClient<IDiscountService, DiscountService>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
builder.Services.AddScoped<IDiscountService, DiscountService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{   
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1));
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(2, TimeSpan.FromMinutes(5));
}