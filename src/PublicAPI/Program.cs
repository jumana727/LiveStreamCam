using System.Diagnostics;
using System.Text.Json.Serialization;
using ApplicationCore;
using Infrastructure.Data;
using PublicAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

if (Debugger.IsAttached)
{
    builder.Logging.ClearProviders();
    builder.Logging.AddDebug();
}
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
    );

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost4200",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
        });
});


var connectionString = builder.Configuration["ConnectionString"]
    ?? throw new($"ConnectionString is empty.");
builder.Services.AddApplicationDbContext(connectionString);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

builder.Services.AddHttpClient();

builder.Services.AddScoped<VideoStreamService>();
builder.Services.AddScoped<AnalyticsSettingsService>();
builder.Services.AddScoped<AnalyticsConfigService>();

builder.Services.AddKeyedScoped<IAnalyticsService, FaceDetectionService>(AnalyticsType.FaceDetection);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowLocalhost4200");

app.MapHub<AnalyticsResultsHub>("/analyticsResultsHub");

app.Run();

public partial class Program { }
