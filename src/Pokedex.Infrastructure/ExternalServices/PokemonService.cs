using Pokedex.Domain.Entities;
using Pokedex.Domain.Interfaces;
using System.Text.Json;

namespace Pokedex.Infrastructure.ExternalServices
{
    public class PokemonService : IPokemonService
    {
        private readonly HttpClient _httpClient;

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Pokemon> GetPokemonAsync(string idOrName)
        {
            var response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{idOrName}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Pokemon not found.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(json);

            var id = data.GetProperty("id").GetInt32();
            var name = data.GetProperty("name").GetString();
            var sprite = data.GetProperty("sprites").GetProperty("front_default").GetString();
            var cries = data.GetProperty("cries").GetProperty("latest").GetString();

            return new Pokemon
            {
                Id = id,
                Name = name,
                Sprite = sprite,
                Cries = cries,
                //Evolutions = evolutions
            };
        }
    }
}