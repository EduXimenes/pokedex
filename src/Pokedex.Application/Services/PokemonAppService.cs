using Pokedex.Application.Dto;
using Pokedex.Application.Interfaces;
using Pokedex.Domain.Interfaces;

namespace Pokedex.Application.Services
{
    public class PokemonAppService : IPokemonAppService
    {
        private readonly IPokemonService _pokemonService;

        public PokemonAppService(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }
        public async Task<PokemonDto> GetByIdOrNameAsync(string idOrName)
        {
            var pokemon = await _pokemonService.GetPokemonAsync(idOrName);

            return new PokemonDto
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Sprite = pokemon.Sprite,
                Cries = pokemon.Cries,
                Evolutions = pokemon.Evolutions
            };
        }
    }
}
