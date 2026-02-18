using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// CORS - Allowing origins
// https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-9.0
// https://developer.mozilla.org/en-US/docs/Web/HTTP/Guides/CORS

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173", "http://92.33.158.78", "http://emilrundberg.se", "http://www.emilrundberg.se", "http://192.168.10.174", "http://192.168.10.100", "http://192.168.10.174", "http://192.168.10.100");
                      });
});

// Build app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();





app.UseCors(MyAllowSpecificOrigins);

app.Run();
