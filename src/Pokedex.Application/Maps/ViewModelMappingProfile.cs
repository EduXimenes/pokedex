using AutoMapper;
using Pokedex.Application.ViewModels;
using Pokedex.Domain.Dto;
using Pokedex.Domain.Entities;

namespace Pokedex.API.Mapping
{
    public class ViewModelMappingProfile : Profile
    {
        public ViewModelMappingProfile()
        {
            CreateMap<PokemonMaster, PokemonMasterViewModel>();
            CreateMap<CreatePokemonMasterViewModel, PokemonMasterDto>();
            CreateMap<CapturedPokemon, CapturedPokemonViewModel>();
            CreateMap<Pokemon, PokemonViewModel>();
            CreateMap<CapturePokemonViewModel, CapturePokemonDto>();
        }
    }
} 