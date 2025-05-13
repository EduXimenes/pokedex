using System.Text.Json;

namespace Pokedex.Infrastructure.ExternalServices
{
    public interface IPokemonApiClient
    {
        Task<JsonElement> GetPokemonDataAsync(string idOrName);
        Task<JsonElement> GetEvolutionChainDataAsync(string evolutionUrl);
        Task<JsonElement> GetSpeciesDataAsync(string speciesUrl);
    }
}
