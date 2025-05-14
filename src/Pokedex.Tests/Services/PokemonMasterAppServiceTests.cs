using Microsoft.EntityFrameworkCore;
using Moq;
using Pokedex.Application.Services;
using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;
using Pokedex.Infrastructure.Data;
using Pokedex.Infrastructure.ExternalServices;
using System.Text.Json;
using Xunit;

namespace Pokedex.Tests.Application.Services;

public class PokemonMasterAppServiceTests
{
    private readonly Mock<IPokemonApiClient> _mockPokemonApiClient;
    private readonly PokedexDbContext _context;
    private readonly PokemonMasterAppService _service;

    public PokemonMasterAppServiceTests()
    {
        var options = new DbContextOptionsBuilder<PokedexDbContext>()
            .UseInMemoryDatabase(databaseName: "PokedexTestDb")
            .Options;

        _context = new PokedexDbContext(options);
        _mockPokemonApiClient = new Mock<IPokemonApiClient>();
        _service = new PokemonMasterAppService(_context, _mockPokemonApiClient.Object);
    }

    [Fact]
    public async Task CreateMasterAsync_ShouldCreateNewMaster_WhenValidDataProvided()
    {
        var dto = new PokemonMasterDto
        {
            Name = "Edu Ketchum",
            Email = "Edu@hot.com",
            Document = "123.456.789-10",
            Age = 10
        };

        var result = await _service.CreateMasterAsync(dto);

        Assert.True(result > 0);
        var createdMaster = await _context.PokemonMasters.FindAsync(result);
        Assert.NotNull(createdMaster);
        Assert.Equal(dto.Name, createdMaster.Name);
        Assert.Equal(dto.Email, createdMaster.Email);
    }

    [Fact]
    public async Task CapturePokemonAsync_ShouldCapturePokemon_WhenValidDataProvided()
    {
        var master = new PokemonMaster("Edu", "Edu@hot.com", "123.456.789-10", 10);
        _context.PokemonMasters.Add(master);
        await _context.SaveChangesAsync();

        var dto = new CapturePokemonDto
        {
            MasterId = master.Id,
            PokemonId = 1
        };

        var jsonElement = CreateMockJsonElement();
        _mockPokemonApiClient
            .Setup(x => x.GetPokemonDataAsync(dto.PokemonId.ToString()))
            .ReturnsAsync(jsonElement);

        await _service.CapturePokemonAsync(dto);

        var capturedPokemon = await _context.CapturedPokemons
            .FirstOrDefaultAsync(p => p.PokemonMasterId == master.Id);

        Assert.NotNull(capturedPokemon);
        Assert.Equal(dto.PokemonId, capturedPokemon.PokemonId);
        Assert.Equal("bulbasaur", capturedPokemon.Name);
    }

    [Fact]
    public async Task GetCapturedPokemonsAsync_ShouldReturnList_WhenMasterHasPokemons()
    {
        var master = new PokemonMaster("Edu", "Edu@hot.com", "123.456.789-10", 10);
        _context.PokemonMasters.Add(master);
        await _context.SaveChangesAsync();

        var capturedPokemon = new CapturedPokemon
        {
            PokemonId = 1,
            PokemonMasterId = master.Id,
            Name = "bulbasaur",
            Sprite = "sprite_url"
        };
        _context.CapturedPokemons.Add(capturedPokemon);
        await _context.SaveChangesAsync();

        var result = await _service.GetCapturedPokemonsAsync(master.Id);

        Assert.Single(result);
        Assert.Equal(capturedPokemon.Name, result[0].Name);
    }

    [Fact]
    public async Task GetMasterAsync_ShouldReturnMaster_WhenValidIdProvided()
    {
        var master = new PokemonMaster("Edu", "Edu@hot.com", "123.456.789-10", 10);
        _context.PokemonMasters.Add(master);
        await _context.SaveChangesAsync();

        var result = await _service.GetMasterAsync(master.Id.ToString());

        Assert.NotNull(result);
        Assert.Equal(master.Name, result.Name);
        Assert.Equal(master.Email, result.Email);
    }

    [Fact]
    public async Task GetMasterAsync_ShouldThrowException_WhenMasterNotFound()
    {
        await Assert.ThrowsAsync<Exception>(() => _service.GetMasterAsync("999"));
    }

    private JsonElement CreateMockJsonElement()
    {
        var jsonString = @"{
            ""id"": 1,
            ""name"": ""bulbasaur"",
            ""sprites"": {
                ""front_default"": ""https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png""
            }
        }";
        return JsonDocument.Parse(jsonString).RootElement;
    }
}