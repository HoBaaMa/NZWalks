using AutoMapper;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Mappings
{
    public class DifficultyMappingProfile : Profile
    {
        public DifficultyMappingProfile()
        {
            CreateMap<DifficultyDto, Difficulty>().ReverseMap();
        }
    }
}
