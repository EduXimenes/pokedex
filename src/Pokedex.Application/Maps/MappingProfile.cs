using AutoMapper;
using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;

namespace Pokedex.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pokemon, PokemonDto>().ReverseMap();

        }
    }
}