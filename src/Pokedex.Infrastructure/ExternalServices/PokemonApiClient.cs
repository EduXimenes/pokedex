using System.Text.Json;


namespace Pokedex.Infrastructure.ExternalServices
{
    public class PokemonApiClient : IPokemonApiClient
    {
        private readonly HttpClient _httpClient;

        public PokemonApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JsonElement> GetEvolutionChainDataAsync(string evolutionUrl)
        {
            var response = await _httpClient.GetAsync($"{evolutionUrl}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(stream);

            return json.RootElement.Clone();
        }
        public async Task<JsonElement> GetSpeciesDataAsync(string speciesUrl)
        {
            var response = await _httpClient.GetAsync($"{speciesUrl}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(stream);

            return json.RootElement.Clone();
        }
        public async Task<JsonElement> GetPokemonDataAsync(string idOrName)
        {
            var response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{idOrName}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(stream);
            
            return json.RootElement.Clone();
        }
    }
}
