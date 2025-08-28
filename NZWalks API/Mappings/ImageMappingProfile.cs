using AutoMapper;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Mappings
{
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<ImageUploadRequestDto, Image>().ForMember(dest => dest.FileExtension, opt => opt.MapFrom(src => Path.GetExtension(src.File.FileName)))
              .ForMember(dest => dest.FileSizeInBytes, opt => opt.MapFrom(src => src.File.Length));
        }
    }
}
