using Pokedex.Application.Dto;

namespace Pokedex.Application.Interfaces
{
    public interface IPokemonAppService
    {
        Task<PokemonDto> GetByIdOrNameAsync(string idOrName);
    }
}
