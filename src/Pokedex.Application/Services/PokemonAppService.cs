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
        var pokemon = await GetByIdAsync(idOrName);
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
                Console.WriteLine($"Error fetch Pokemon with Id {ids.Last()}: {ex.Message}");
            }
        }
        return pokemons;
    }
    #endregion

    #region Private Methods
    private async Task<List<Pokemon>> GetEvolutionsAsync(JsonElement data, string speciesUrl)
    {
        var evolutionChain = new List<string>();

        var speciesData = await _pokemonApiClient.GetSpeciesDataAsync(speciesUrl);
        var evolutionUrl = GetEvolutionUrl(speciesData);
        var evolutionData = await _pokemonApiClient.GetEvolutionChainDataAsync(evolutionUrl);

        var chain = evolutionData.GetProperty("chain");
        var name = data.GetProperty("name").ToString();

        GetEvolutions(chain, evolutionChain, name);

        var evolvedPokemons = new List<Pokemon>();
        await GetDataEvolvedPokemons(evolutionChain, evolvedPokemons);

        return evolvedPokemons;
    }
    private async Task GetDataEvolvedPokemons(List<string> evolutionChain, List<Pokemon> evolvedPokemons)
    {
        foreach (var evoName in evolutionChain)
        {
            var evoData = await _pokemonApiClient.GetPokemonDataAsync(evoName);
            var spriteUrl = GetSpriteUrl(evoData);
            var spriteBase64 = await GetSpriteBase64WithRetry(spriteUrl);
            var dto = CreatePokemonDto(evoData, spriteUrl, spriteBase64);
            evolvedPokemons.Add(_mapper.Map<Pokemon>(dto));
        }
    }
    private void GetEvolutions(JsonElement node, List<string> result, string pokemonName)
    {
        var speciesName = node.GetProperty("species").GetProperty("name").GetString();

        if (speciesName != pokemonName && !string.IsNullOrEmpty(speciesName))
        {
            result.Add(speciesName);
        }

        foreach (var evolution in node.GetProperty("evolves_to").EnumerateArray())
        {
            GetEvolutions(evolution, result, pokemonName);
        }
    }
    private async Task<Pokemon> GetByIdAsync(string id)
    {
        try
        {
            var data = await _pokemonApiClient.GetPokemonDataAsync(id);
            var spriteUrl = GetSpriteUrl(data);
            var spriteBase64 = await GetSpriteBase64WithRetry(spriteUrl);
            var species = GetSpeciesUrl(data);
            var evolutions = await GetEvolutionsAsync(data, species);

            var dto = CreatePokemonDto(data, spriteUrl, spriteBase64, evolutions);
            return _mapper.Map<Pokemon>(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Pokemon data: {ex.Message}");
            throw;
        }
    }
    private string GetSpriteUrl(JsonElement data)
    {
        return data.GetProperty("sprites")
            .GetProperty("front_default")
            .GetString()!;
    }
    private string GetSpeciesUrl(JsonElement data)
    {
        return data.GetProperty("species")
            .GetProperty("url")
            .GetString()!;
    }
    private string GetEvolutionUrl(JsonElement data)
    {
        return data.GetProperty("evolution_chain")
            .GetProperty("url")
            .GetString()!;
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
    private async Task<string> GetSpriteBase64Async(string spriteUrl)
    {
        try
        {
            var imageBytes = await _httpClient.GetByteArrayAsync(spriteUrl);
            return Convert.ToBase64String(imageBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting to base64: {ex.Message}");
            return null;
        }
    }
    private PokemonDto CreatePokemonDto(JsonElement data, string spriteUrl, string? spriteBase64, List<Pokemon>? evolutions = null)
    {
        return new PokemonDto
        {
            Id = data.GetProperty("id").GetInt32(),
            Name = data.GetProperty("name").GetString()!,
            Sprite = spriteUrl,
            Cries = data.GetProperty("cries").GetProperty("latest").GetString(),
            SpriteBase64 = spriteBase64,
            Evolutions = evolutions ?? new List<Pokemon>()
        };
    }
    #endregion
}