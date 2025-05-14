using Microsoft.AspNetCore.Mvc;
using Pokedex.Application.Interfaces;
using Pokedex.Domain.Dto;
using Pokedex.Application.ViewModels;
using AutoMapper;

namespace Pokedex.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PokemonController : ControllerBase
{
    private readonly IPokemonAppService _pokemonAppService;
    private readonly IMapper _mapper;

    public PokemonController(IPokemonAppService pokemonAppService, IMapper mapper)
    {
        _pokemonAppService = pokemonAppService;
        _mapper = mapper;
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
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string idOrName)
    {
        if (string.IsNullOrWhiteSpace(idOrName))
        {
            return BadRequest(ApiResponse<object>.Error(
                new List<string> { "The Pokémon name or ID must be provided." },
                "Invalid request"));
        }

        try
        {
            var pokemon = await _pokemonAppService.GetByIdOrNameAsync(idOrName);
            var viewModel = _mapper.Map<PokemonViewModel>(pokemon);
            return Ok(ApiResponse<object>.Ok(viewModel, "Pokémon retrieved successfully"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<object>.Error(
                new List<string> { ex.Message },
                "Pokémon not found"));
        }
    }

    /// <summary>
    /// Retrieves basic data about a 10 random Pokémons.
    /// </summary>
    /// <returns>A list of 10 random Pokemon with their details.</returns>
    /// <response code="200">Returns the list of random Pokemon.</response>
    /// <response code="400">If there was an error retrieving the Pokemon.</response>
    [HttpGet("random")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRandom()
    {
        try
        {
            var pokemons = await _pokemonAppService.GetRandomAsync();
            var viewModels = _mapper.Map<List<PokemonViewModel>>(pokemons);
            return Ok(ApiResponse<object>.Ok(viewModels, "Random Pokémon retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Error(
                new List<string> { ex.Message },
                "Failed to retrieve random Pokémon"));
        }
    }
}