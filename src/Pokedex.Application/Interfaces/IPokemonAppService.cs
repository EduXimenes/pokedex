using Pokedex.Domain.Entities;

namespace Pokedex.Application.Interfaces
{
    public interface IPokemonAppService
    {
        Task<Pokemon> GetByIdOrNameAsync(string idOrName);
        Task<List<Pokemon>> GetRandomAsync();
    }
}
