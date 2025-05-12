using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Interfaces;
using Pokedex.Domain.Dto;

[ApiController]
[Route("api/[controller]")]
public class PokemonMasterController : ControllerBase
{
    private readonly IPokemonMasterAppService _pokemonMasterAppService;

    public PokemonMasterController(IPokemonMasterAppService pokemonMasterAppService)
    {
        _pokemonMasterAppService = pokemonMasterAppService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateMaster([FromBody] PokemonMasterDto dto)
    {
        var id = await _pokemonMasterAppService.CreateMasterAsync(dto);
        return Ok(new { MasterId = id });
    }
    [HttpGet("{masterName}")]
    public async Task<IActionResult> GetMaster(string masterName)
    {
        var pokemons = await _pokemonMasterAppService.GetMasterAsync(masterName);
        return Ok(pokemons);
    }
    [HttpPost("capture")]
    public async Task<IActionResult> CapturePokemon([FromBody] CapturePokemonDto dto)
    {
        await _pokemonMasterAppService.CapturePokemonAsync(dto);
        return Ok("Pokémon capturado com sucesso!");
    }

    [HttpGet("{masterId}/captured")]
    public async Task<IActionResult> GetCapturedPokemons(int masterId)
    {
        var pokemons = await _pokemonMasterAppService.GetCapturedPokemonsAsync(masterId);
        return Ok(pokemons);
    }
}
