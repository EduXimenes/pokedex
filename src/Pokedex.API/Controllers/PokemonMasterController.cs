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

    /// <summary>
    /// Creates a new Pokemon Master.
    /// </summary>
    /// <param name="dto">The Pokemon Master data including name, email, document, and age.</param>
    /// <returns>The ID of the created Pokemon Master.</returns>
    /// <response code="200">Returns the newly created Pokemon Master ID.</response>
    /// <response code="400">If the input data is invalid or creation fails.</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMaster([FromBody] PokemonMasterDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return BadRequest(ApiResponse<object>.Error(errors, "Validation failed"));
        }

        try
        {
            var id = await _pokemonMasterAppService.CreateMasterAsync(dto);
            return Ok(ApiResponse<object>.Ok(new { MasterId = id }, "Pokemon Master created successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Error(new List<string> { ex.Message }, "Failed to create Pokemon Master"));
        }
    }

    /// <summary>
    /// Retrieves a Pokemon Master by ID or name.
    /// </summary>
    /// <param name="masterIdOrName">The Pokemon Master's ID or name.</param>
    /// <returns>The Pokemon Master's information including captured Pokemon.</returns>
    /// <response code="200">Returns the Pokemon Master data.</response>
    /// <response code="404">If the Pokemon Master is not found.</response>
    [HttpGet("{masterIdOrName}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMaster(string masterIdOrName)
    {
        try
        {
            var master = await _pokemonMasterAppService.GetMasterAsync(masterIdOrName);
            return Ok(ApiResponse<object>.Ok(master, "Pokemon Master retrieved successfully"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<object>.Error(new List<string> { ex.Message }, "Pokemon Master not found"));
        }
    }

    /// <summary>
    /// Captures a Pokemon for a Pokemon Master.
    /// </summary>
    /// <param name="dto">The capture data including Pokemon ID, name, sprite, and Master ID.</param>
    /// <returns>A success message indicating the Pokemon was captured.</returns>
    /// <response code="200">Returns a success message.</response>
    /// <response code="400">If the capture operation fails.</response>
    [HttpPost("capture")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CapturePokemon([FromBody] CapturePokemonDto dto)
    {
        try
        {
            await _pokemonMasterAppService.CapturePokemonAsync(dto);
            return Ok(ApiResponse<object>.Ok(null, "Pokémon captured successfully!"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Error(new List<string> { ex.Message }, "Failed to capture Pokémon"));
        }
    }

    /// <summary>
    /// Retrieves all Pokemon captured by a specific Pokemon Master.
    /// </summary>
    /// <param name="masterId">The ID of the Pokemon Master.</param>
    /// <returns>A list of captured Pokemon.</returns>
    /// <response code="200">Returns the list of captured Pokemon.</response>
    /// <response code="404">If the Pokemon Master is not found or has no captured Pokemon.</response>
    [HttpGet("{masterId}/captured")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCapturedPokemons(int masterId)
    {
        try
        {
            var pokemons = await _pokemonMasterAppService.GetCapturedPokemonsAsync(masterId);
            return Ok(ApiResponse<object>.Ok(pokemons, "Captured Pokémon retrieved successfully"));
        }
        catch (Exception ex)
        {
            return NotFound(ApiResponse<object>.Error(new List<string> { ex.Message }, "Failed to retrieve captured Pokémon"));
        }
    }
}
