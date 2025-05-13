using AutoMapper;
using Pokedex.Application.Interfaces;
using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;
using Pokedex.Infrastructure.ExternalServices;
using System.Net.Http;
using System.Text.Json;

namespace Pokedex.Application.Services;

public class PokemonAppService : IPokemonAppService
{
    private readonly IPokemonApiClient _pokemonApiClient;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    public PokemonAppService(IPokemonApiClient pokemonApiClient, IMapper mapper, HttpClient httpClient)
    {
        _pokemonApiClient = pokemonApiClient;
        _mapper = mapper;
        _httpClient = httpClient;
    }
    #region Public Methods
    public async Task<Pokemon> GetByIdOrNameAsync(string idOrName)
    {
        var data = await GetByIdAsync(idOrName);
        var pokemon = _mapper.Map<Pokemon>(data);

        return pokemon;
    }

    public async Task<List<Pokemon>> GetRandomAsync()
    {
        var rand = new Random();
        var ids = new HashSet<int>();
        var pokemons = new List<Pokemon>();

        while (pokemons.Count < 10)
        {
            try
            {
                var randomId = rand.Next(1, 1026);
                if (ids.Add(randomId))
                {
                    var pokemon = await GetByIdAsync(randomId.ToString());
                    pokemons.Add(pokemon);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar Pokemon com Id {ids.Last()}: {ex.Message}");
            }
        }
        return pokemons;
    }
    #endregion

    #region Private Methods
    private async Task<string> GetSpriteBase64Async(string spriteUrl)
    {
        try
        {
            var imageBytes = await _httpClient.GetByteArrayAsync(spriteUrl);
            return Convert.ToBase64String(imageBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao converter sprite para base64: {ex.Message}");
            return null;
        }
    }

    private async Task<Pokemon> GetByIdAsync(string id)
    {
        try
        {
            var data = await _pokemonApiClient.GetPokemonDataAsync(id);
            var spriteUrl = GetSpriteUrl(data);
            var spriteBase64 = await GetSpriteBase64WithRetry(spriteUrl);

            var dto = CreatePokemonDto(data, spriteUrl, spriteBase64);
            return _mapper.Map<Pokemon>(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter dados do Pokémon: {ex.Message}");
            throw;
        }
    }

    private string GetSpriteUrl(JsonElement data)
    {
        return data.GetProperty("sprites")
            .GetProperty("front_default")
            .GetString();
    }

    private async Task<string?> GetSpriteBase64WithRetry(string? spriteUrl)
    {
        if (string.IsNullOrEmpty(spriteUrl))
        {
            return null;
        }

        string? spriteBase64 = await GetSpriteBase64Async(spriteUrl);

        if (spriteBase64 == null)
        {
            await Task.Delay(1000);
            spriteBase64 = await GetSpriteBase64Async(spriteUrl);
        }

        return spriteBase64;
    }

    private PokemonDto CreatePokemonDto(JsonElement data, string spriteUrl, string? spriteBase64)
    {
        return new PokemonDto
        {
            Id = data.GetProperty("id").GetInt32(),
            Name = data.GetProperty("name").GetString()!,
            Sprite = spriteUrl,
            Cries = data.GetProperty("cries").GetProperty("latest").GetString(),
            SpriteBase64 = spriteBase64
        };
    }
    #endregion
}
