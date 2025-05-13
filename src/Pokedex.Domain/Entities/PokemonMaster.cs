namespace Pokedex.Domain.Entities
{
    public class PokemonMaster
    {
        public int Id { get; private set; }
        public string? Name { get; private set; }
        public string? Email { get; private set; }
        public string? Document { get; private set; }
        public int Age { get; private set; }
        public ICollection<CapturedPokemon> CapturedPokemons { get; private set; } = new List<CapturedPokemon>();

        public PokemonMaster() {}
        public PokemonMaster(string name, string email, string document, int age)
        {
            Name = name;
            Email = email;
            Document = document;
            Age = age;
        }
        public void CapturePokemon(CapturedPokemon pokemon)
        {
            CapturedPokemons.Add(pokemon);
        }
    }
}
