namespace Pokedex.Application.ViewModels
{
    public class PokemonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sprite { get; set; }
        public string SpriteBase64 { get; set; }
        public string Cries { get; set; }
        public List<PokemonViewModel> Evolutions { get; set; } = new();
    }
} 