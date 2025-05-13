using Microsoft.EntityFrameworkCore;
using Pokedex.Application.Interfaces;
using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;
using Pokedex.Infrastructure.Data;

namespace Pokedex.Application.Services;
public class PokemonMasterAppService : IPokemonMasterAppService
{
    private readonly PokedexDbContext _context;

    public PokemonMasterAppService(PokedexDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateMasterAsync(PokemonMasterDto dto)
    {
        var master = new PokemonMaster(dto.Name, dto.Email, dto.Document, dto.Age);

        _context.PokemonMasters.Add(master);
        await _context.SaveChangesAsync();
        return master.Id;
    }

    public async Task CapturePokemonAsync(CapturePokemonDto dto)
    {
        var capture = new CapturedPokemon
        {
            PokemonId = dto.PokemonId,
            Name = dto.Name,
            Sprite = dto.Sprite,
            PokemonMasterId = dto.MasterId
        };
        _context.CapturedPokemons.Add(capture);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CapturedPokemon>> GetCapturedPokemonsAsync(int masterId)
    {
        return await _context.CapturedPokemons
            .Where(p => p.PokemonMasterId == masterId)
            .ToListAsync();
    }
    public async Task<PokemonMaster> GetMasterAsync(string masterIdOrName)
    {
        PokemonMaster? response;

        if (int.TryParse(masterIdOrName, out int id))
        {
            response = await _context.PokemonMasters
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        else
        {
            response = await _context.PokemonMasters
                .FirstOrDefaultAsync(p => p.Name.ToLower() == masterIdOrName.ToLower());
        }

        if (response == null)
        {
            throw new Exception("Pokemon Master not Found");
        }

        return response;
    }
}
