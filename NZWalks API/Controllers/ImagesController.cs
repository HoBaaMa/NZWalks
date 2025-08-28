using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;

namespace NZWalks_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;

        public ImagesController(IMapper mapper, IImageRepository imageRepository)
        {
            this._mapper = mapper;
            this._imageRepository = imageRepository;
        }
        // POST: /api/Images/Upload
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
            ValidateFileUpload(imageUploadRequestDto);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Map/Convert The DTO to Domain Model
            var image = _mapper.Map<Image>(imageUploadRequestDto);

            // Use repository to upload image
            await _imageRepository.Upload(image);

            return Ok(image);
        }

        private void ValidateFileUpload(ImageUploadRequestDto file)
        {
            string[] fileExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!fileExtensions.Contains(Path.GetExtension(file.File.FileName)))
            {
                ModelState.AddModelError("file", "Unspported file extension.");
            }
            
            if (file.File.Length > 10485760) // 10MB
            {
                ModelState.AddModelError("file", "File size more than 10MB.");
            }
        }
    }
}
