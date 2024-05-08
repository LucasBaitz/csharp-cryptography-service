using Cryptography.API.Data;
using Cryptography.API.Repositories;
using Cryptography.Domain.Entities;
using Cryptography.Domain.Interfaces;
using Cryptography.Services.Interfaces;
using Cryptography.Services.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("SqliteConnectionString");
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    opts.UseSqlite(connectionString);
});

builder.Services.Configure<CryptographySettings>(builder.Configuration.GetSection("CryptographySettings"));
builder.Services.AddScoped<ICryptographyService, CryptographyService>();

builder.Services.AddScoped<IRepository<CryptData>, CryptDataRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
