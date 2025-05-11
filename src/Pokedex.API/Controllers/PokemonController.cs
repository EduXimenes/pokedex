using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Interfaces;

namespace Pokedex.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PokemonController : ControllerBase
{
    private readonly IPokemonAppService _pokemonAppService;

    public PokemonController(IPokemonAppService pokemonAppService)
    {
        _pokemonAppService = pokemonAppService;
    }

    /// <summary>
    /// Retrieves basic data about a Pokémon by ID or name.
    /// </summary>
    /// <param name="idOrName">The Pokémon ID (1, 2, 3...) or name (bulbasaur, charizard, dragonite...).</param>
    /// <returns>Basic Pokémon information including evolutions and sprite.</returns>
    /// <response code="200">Returns the Pokémon data.</response>
    /// <response code="400">If the idOrName is null or empty.</response>
    /// <response code="404">If the Pokémon is not found.</response>
    [HttpGet("{idOrName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string idOrName)
    {
        if (string.IsNullOrWhiteSpace(idOrName))
        {
            return BadRequest("The Pokémon name or ID must be provided.");
        }

        try
        {
            var pokemon = await _pokemonAppService.GetByIdOrNameAsync(idOrName);
            return Ok(pokemon);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}