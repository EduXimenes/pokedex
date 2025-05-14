using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using Pokedex.Application.Services;
using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;
using Pokedex.Infrastructure.ExternalServices;
using System.Net;
using System.Text.Json;
using Xunit;

namespace Pokedex.Tests.Application.Services;

public class PokemonAppServiceTests
{
    private readonly Mock<IPokemonApiClient> _mockPokemonApiClient;
    private readonly Mock<IMapper> _mockMapper;
    private readonly HttpClient _httpClient;
    private readonly PokemonAppService _service;

    public PokemonAppServiceTests()
    {
        _mockPokemonApiClient = new Mock<IPokemonApiClient>();
        _mockMapper = new Mock<IMapper>();

        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ByteArrayContent(new byte[] { 1, 2, 3 })
            });

        _httpClient = new HttpClient(handler.Object);
        _service = new PokemonAppService(_mockPokemonApiClient.Object, _mockMapper.Object, _httpClient);
    }

    [Fact]
    public async Task GetByIdOrNameAsync_ShouldReturnPokemon_WhenValidIdProvided()
    {
        var pokemonId = "1";
        var jsonElement = CreateMockJsonElement();
        var speciesJson = CreateMockSpeciesJson();
        var evolutionJson = CreateMockEvolutionJson();
        var pokemonDto = CreateMockPokemonDto();
        var expectedPokemon = CreateMockPokemon();

        _mockPokemonApiClient
            .Setup(x => x.GetPokemonDataAsync(pokemonId))
            .ReturnsAsync(jsonElement);

        _mockPokemonApiClient
            .Setup(x => x.GetSpeciesDataAsync(It.IsAny<string>()))
            .ReturnsAsync(speciesJson);

        _mockPokemonApiClient
            .Setup(x => x.GetEvolutionChainDataAsync(It.IsAny<string>()))
            .ReturnsAsync(evolutionJson);

        _mockPokemonApiClient
            .Setup(x => x.GetPokemonDataAsync(It.Is<string>(s => s != pokemonId)))
            .ReturnsAsync(jsonElement);

        _mockMapper
            .Setup(x => x.Map<Pokemon>(It.IsAny<PokemonDto>()))
            .Returns(expectedPokemon);

        var result = await _service.GetByIdOrNameAsync(pokemonId);

        Assert.NotNull(result);
        Assert.Equal(expectedPokemon.Id, result.Id);
        Assert.Equal(expectedPokemon.Name, result.Name);
        Assert.Equal(expectedPokemon.Sprite, result.Sprite);
        _mockPokemonApiClient.Verify(x => x.GetPokemonDataAsync(pokemonId), Times.Once);
    }

    [Fact]
    public async Task GetByIdOrNameAsync_ShouldThrowException_WhenApiCallFails()
    {
        var pokemonId = "999";
        _mockPokemonApiClient
            .Setup(x => x.GetPokemonDataAsync(pokemonId))
            .ThrowsAsync(new HttpRequestException("API Error"));

        await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetByIdOrNameAsync(pokemonId));
    }

    [Fact]
    public async Task GetRandomAsync_ShouldReturnListOfPokemons()
    {
        var jsonElement = CreateMockJsonElement();
        var speciesJson = CreateMockSpeciesJson();
        var evolutionJson = CreateMockEvolutionJson();
        var pokemonDto = CreateMockPokemonDto();
        var expectedPokemon = CreateMockPokemon();

        _mockPokemonApiClient
            .Setup(x => x.GetPokemonDataAsync(It.IsAny<string>()))
            .ReturnsAsync(jsonElement);

        _mockPokemonApiClient
            .Setup(x => x.GetSpeciesDataAsync(It.IsAny<string>()))
            .ReturnsAsync(speciesJson);

        _mockPokemonApiClient
            .Setup(x => x.GetEvolutionChainDataAsync(It.IsAny<string>()))
            .ReturnsAsync(evolutionJson);

        _mockMapper
            .Setup(x => x.Map<Pokemon>(It.IsAny<PokemonDto>()))
            .Returns(expectedPokemon);

        var result = await _service.GetRandomAsync();

        Assert.NotNull(result);
        Assert.Equal(10, result.Count);
        _mockPokemonApiClient.Verify(x => x.GetPokemonDataAsync(It.IsAny<string>()), Times.AtLeast(10));
    }

    #region Private Methods
    private JsonElement CreateMockJsonElement()
    {
        var jsonString = @"{
            ""id"": 1,
            ""name"": ""bulbasaur"",
            ""sprites"": {
                ""front_default"": ""https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png""
            },
            ""cries"": {
                ""latest"": ""https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/cries/1.ogg""
            },
            ""species"": {
                ""url"": ""https://pokeapi.co/api/v2/pokemon-species/1/""
            }
        }";
        return JsonDocument.Parse(jsonString).RootElement;
    }

    private JsonElement CreateMockSpeciesJson()
    {
        var jsonString = @"{
            ""evolution_chain"": {
                ""url"": ""https://pokeapi.co/api/v2/evolution-chain/1/""
            }
        }";
        return JsonDocument.Parse(jsonString).RootElement;
    }

    private JsonElement CreateMockEvolutionJson()
    {
        var jsonString = @"{
            ""chain"": {
                ""species"": {
                    ""name"": ""bulbasaur""
                },
                ""evolves_to"": [
                    {
                        ""species"": {
                            ""name"": ""ivysaur""
                        },
                        ""evolves_to"": [
                            {
                                ""species"": {
                                    ""name"": ""venusaur""
                                },
                                ""evolves_to"": []
                            }
                        ]
                    }
                ]
            }
        }";
        return JsonDocument.Parse(jsonString).RootElement;
    }

    private PokemonDto CreateMockPokemonDto()
    {
        return new PokemonDto
        {
            Id = 1,
            Name = "bulbasaur",
            Sprite = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png",
            Cries = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/cries/1.ogg",
            SpriteBase64 = "base64string",
            Evolutions = new List<Pokemon>()
        };
    }

    private Pokemon CreateMockPokemon()
    {
        return new Pokemon(
            1,
            "bulbasaur",
            "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/1.png",
            "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/cries/1.ogg"
        );
    }
    #endregion
}
