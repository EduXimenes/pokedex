using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;

namespace Pokedex.Application.Interfaces
{
    public interface IPokemonMasterAppService
    {
        Task CapturePokemonAsync(CapturePokemonDto dto);
        Task<int> CreateMasterAsync(PokemonMasterDto dto);
        Task<List<CapturedPokemon>> GetCapturedPokemonsAsync(int masterId);
        Task<PokemonMaster> GetMasterAsync(string masterIdOrName);
    }
}