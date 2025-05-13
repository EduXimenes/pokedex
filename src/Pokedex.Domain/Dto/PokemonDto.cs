using Pokedex.Domain.Entities;

namespace Pokedex.Domain.Dto
{
    public class PokemonDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Sprite { get; set; }
        public string? SpriteBase64 { get; set; }
        public string? Cries { get; set; }
        public List<Pokemon>? Evolutions { get; set; } = new List<Pokemon>();
    }
}
