using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;

namespace Pokedex.Application.Interfaces
{
    public interface IPokemonMasterAppService
    {
        Task CapturePokemonAsync(CapturePokemonDto capturedPokemon);
        Task<int> CreateMasterAsync(PokemonMasterDto pokemonMaster);
        Task<List<CapturedPokemon>> GetCapturedPokemonsAsync(int masterId);
        Task<PokemonMaster> GetMasterAsync(string masterIdOrName);
    }
}