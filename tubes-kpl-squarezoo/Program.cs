using tubes_kpl_squarezoo;
using tubes_kpl_squarezoo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Tentukan path ke folder data
string reportPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "reports.json");

// Daftarkan sebagai Singleton karena service ini pegang state (Dictionary) 
// yang harus konsisten selama aplikasi jalan.
builder.Services.AddSingleton(new ReportService(reportPath));

builder.Services.AddScoped<AdminManager>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
