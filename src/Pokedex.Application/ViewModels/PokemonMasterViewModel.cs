namespace Pokedex.Application.ViewModels
{
    public class PokemonMasterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public int Age { get; set; }
        public List<CapturedPokemonViewModel> CapturedPokemons { get; set; } = new();
    }
} 