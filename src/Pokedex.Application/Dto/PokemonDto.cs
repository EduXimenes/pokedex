namespace Pokedex.Application.Dto
{
    public class PokemonDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Sprite { get; set; }
        public string? Cries { get; set; }
        public List<string> Evolutions { get; set; } = new List<string>();
    }
}
