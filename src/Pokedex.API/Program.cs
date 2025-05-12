using Microsoft.EntityFrameworkCore;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Mapping;
using Pokedex.Application.Services;
using Pokedex.Domain.Interfaces;
using Pokedex.Infrastructure.Data;
using Pokedex.Infrastructure.ExternalServices;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IPokemonAppService, PokemonAppService>();
builder.Services.AddScoped<IPokemonApiClient, PokemonApiClient>();
builder.Services.AddScoped<IPokemonMasterAppService, PokemonMasterAppService>();
builder.Services.AddScoped<PokedexDbContext>();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<PokedexDbContext>(options =>
    options.UseSqlite("Data Source=pokedex.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
