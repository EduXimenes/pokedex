namespace Pokedex.Domain.Entities
{
    public class CapturedPokemon
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
        public string Name { get; set; } = null!;
        public string? Sprite { get; set; }
        public string? Cries { get; set; }
        public int PokemonMasterId { get; set; }
        public PokemonMaster PokemonMaster { get; set; } = null!;
    }
}
