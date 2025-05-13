namespace Pokedex.Domain.Entities
{
    public class Pokemon
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Sprite { get; private set; }
        public string SpriteBase64 { get; private set; }
        public string Cries { get; private set; }
        public List<Pokemon> Evolutions { get; private set; }

        private Pokemon() { }

        public Pokemon(int id, string name, string sprite, string cries, string spriteBase64 = null)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Pokemon ID must be greater than 0", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Pokemon name cannot be empty", nameof(name));
            }

            Id = id;
            Name = name;
            Sprite = sprite;
            Cries = cries;
            SpriteBase64 = spriteBase64;
            Evolutions = new List<Pokemon>();
        }
    }
}