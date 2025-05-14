using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokedex.Application.Interfaces;
using Pokedex.Application.Mapping;
using Pokedex.Application.Services;
using Pokedex.Application.Validators;
using Pokedex.Domain.Dto;
using Pokedex.Infrastructure.Data;
using Pokedex.Infrastructure.ExternalServices;
using System.Reflection;
using Pokedex.API.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(5000);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddFluentValidation(fv => 
    {
        fv.RegisterValidatorsFromAssemblyContaining<CreatePokemonMasterViewModelValidator>();
        fv.AutomaticValidationEnabled = true;
    });

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(e => e.Value.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return new BadRequestObjectResult(ApiResponse<object>.Error(errors, "Validation failed"));
    };
});

builder.Services.AddScoped<IPokemonAppService, PokemonAppService>();
builder.Services.AddScoped<IPokemonApiClient, PokemonApiClient>();
builder.Services.AddScoped<IPokemonMasterAppService, PokemonMasterAppService>();
builder.Services.AddScoped<PokedexDbContext>();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly, typeof(ViewModelMappingProfile).Assembly);
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Pokédex API",
        Version = "v1",
        Description = "API for managing Pokémon and Pokémon Masters"
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<PokedexDbContext>(options =>
    options.UseSqlite("Data Source=pokedex.db"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokédex API V1");
    c.RoutePrefix = "swagger";
});

app.UseAuthorization();
app.MapControllers();
app.Run();
