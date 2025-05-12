using AutoMapper;
using Pokedex.Application.Interfaces;
using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;
using Pokedex.Infrastructure.ExternalServices;

namespace Pokedex.Application.Services;

public class PokemonAppService : IPokemonAppService
{
    private readonly IPokemonApiClient _pokemonApiClient;
    private readonly IMapper _mapper;

    public PokemonAppService(IPokemonApiClient pokemonApiClient, IMapper mapper)
    {
        _pokemonApiClient = pokemonApiClient;
        _mapper = mapper;
    }

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

        private async Task<Pokemon> GetByIdAsync(string id)
        {
        try
        {
            var data = await _pokemonApiClient.GetPokemonDataAsync(id);
            var dto = new PokemonDto
            {
                Id = data.GetProperty("id").GetInt32(),
                Name = data.GetProperty("name").GetString()!,
                Sprite = data.GetProperty("sprites").GetProperty("front_default").GetString(),
                Cries = data.GetProperty("cries").GetProperty("latest").GetString()
            };
            return _mapper.Map<Pokemon>(dto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao obter dados do Pokemon: {ex.Message}");
            throw; 
        }
    }
}
