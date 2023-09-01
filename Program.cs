using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using XunitAssessment.Models;
using XunitAssessment.Services;
using XunitAssessment.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DemoContext>(Options =>
Options.UseSqlServer(builder.Configuration.GetConnectionString("connectstring"))
    );

builder.Services.AddScoped<AotableInterface,AotableRepository>();

builder.Services.AddScoped<AocolumnInterface, AocolumnRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
