namespace Pokedex.Domain.Entities
{
    public class PokemonMaster
    {
        public int Id { get; private set; }
        public string? Name { get; private set; }
        public string? Email { get; private set; }
        public ICollection<CapturedPokemon> CapturedPokemons { get; private set; } = new List<CapturedPokemon>();

        public PokemonMaster() {}
        public PokemonMaster(string name, string email)
        {
            Name = name;
            Email = email;
        }
        public void CapturePokemon(CapturedPokemon pokemon)
        {
            CapturedPokemons.Add(pokemon);
        }
    }
}
