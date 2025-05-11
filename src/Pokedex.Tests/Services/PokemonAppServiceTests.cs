using Moq;
using Xunit;
using Pokedex.Application.Services;
using Pokedex.Domain.Entities;
using Pokedex.Domain.Interfaces;

namespace Pokedex.Tests.Application.Services;

public class PokemonAppServiceTests
{
    [Fact]
    public async Task GetByIdOrNameAsync_ShouldReturnPokemonData()
    {
        // Arrange
        var pokemon = new Pokemon
        {
            Id = 1,
            Name = "bulbasaur",
            Sprite = "spriteUrl",
            Cries = "cryUrl",
        };

        var mockService = new Mock<IPokemonService>();
        mockService.Setup(s => s.GetPokemonAsync("bulbasaur")).ReturnsAsync(pokemon);

        var appService = new PokemonAppService(mockService.Object);

        // Act
        var result = await appService.GetByIdOrNameAsync("bulbasaur");

        // Assert
        Assert.Equal(pokemon.Id, result.Id);
        Assert.Equal(pokemon.Name, result.Name);
        Assert.Equal(pokemon.Sprite, result.Sprite);
        Assert.Equal(pokemon.Cries, result.Cries);
    }

    [Fact]
    public async Task GetByIdOrNameAsync_ShouldThrowErrorIfPokemonNotFound()
    {
        // Arrange
        var mockService = new Mock<IPokemonService>();
        mockService.Setup(s => s.GetPokemonAsync("unknown")).ThrowsAsync(new Exception("Pokemon not found"));

        var appService = new PokemonAppService(mockService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => appService.GetByIdOrNameAsync("unknown"));
    }
}
