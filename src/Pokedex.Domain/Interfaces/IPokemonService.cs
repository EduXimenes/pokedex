using Pokedex.Domain.Entities;

namespace Pokedex.Domain.Interfaces
{
    public interface IPokemonService
    {
        Task<Pokemon> GetPokemonAsync(string idOrName);
        Task<List<Pokemon>> GetRandomPokemonsAsync();

    }
}
