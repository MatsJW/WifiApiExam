using Microsoft.EntityFrameworkCore;
using WifiAPIExam.Database;
using WifiAPIExam.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Wifi Api", Version = "v1" });
});

builder.Services.AddDbContext<WifiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5173") 
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

builder.Services.AddScoped<IImportService, ImportService>();

var app = builder.Build();

app.UseSwagger();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
    app.MapOpenApi();
}

// using (var scope = app.Services.CreateScope())
// {
//     var priceImportService = scope.ServiceProvider.GetRequiredService<IImportService>();
//
//     // Specify your directory path
//     string dir = Path.Combine(Directory.GetCurrentDirectory(), "wifi-usage-2025-04");
//
//     // Run the import asynchronously
//     await priceImportService.ImportFromDirectoryAsync(dir);

// maybe use as a first run kinda thing?
// if (db.Database.EnsureCreated())
// {
//     var importSvc = scope.ServiceProvider.GetRequiredService<IImportService>();
//     string dir = Path.Combine(Directory.GetCurrentDirectory(), "wifi-usage-2025-04");
//     await importSvc.ImportFromDirectoryAsync(dir);
// }
// }

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();