using AutoMapper;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Mappings
{
    public class WalkMappingProfile : Profile
    {
        public WalkMappingProfile()
        {
            CreateMap<AddWalkRequestDto, Walk>();
            CreateMap<WalkDto, Walk>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>();
        }
    }
}
