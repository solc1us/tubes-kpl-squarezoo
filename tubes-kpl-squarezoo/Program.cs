using tubes_kpl_squarezoo;
using tubes_kpl_squarezoo.Services;

var builder = WebApplication.CreateBuilder(args);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Tentukan path ke folder data
string reportPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "reports.json");
builder.Services.AddSingleton(new ReportService(reportPath));

string userPath = Path.Combine(builder.Environment.ContentRootPath, "data", "users.json");
builder.Services.AddSingleton(new UserService(userPath));

builder.Services.AddScoped<AdminManager>();

// Add services to the container.
builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin() // Di tahap development, hajar semua dulu biar gak pusing
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
