using domain.Interfaces;
using infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await context.Database.MigrateAsync();

app.UseResponseCaching();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
