namespace Pokedex.Domain.Dto
{
    public class CapturePokemonDto
    {
        public int MasterId { get; set; }
        public int PokemonId { get; set; }
        public string Name { get; set; }
        public string? Sprite { get; set; }
    }
}
