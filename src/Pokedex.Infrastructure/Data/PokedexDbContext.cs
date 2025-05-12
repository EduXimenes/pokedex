using Microsoft.EntityFrameworkCore;
using Pokedex.Domain.Entities;

namespace Pokedex.Infrastructure.Data
{
    public class PokedexDbContext : DbContext
    {
        public DbSet<PokemonMaster> PokemonMasters => Set<PokemonMaster>();
        public DbSet<CapturedPokemon> CapturedPokemons => Set<CapturedPokemon>();
        public PokedexDbContext(DbContextOptions<PokedexDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonMaster>()
                .HasMany(m => m.CapturedPokemons)
                .WithOne(p => p.PokemonMaster)
                .HasForeignKey(p => p.PokemonMasterId);
        }
    }
}
