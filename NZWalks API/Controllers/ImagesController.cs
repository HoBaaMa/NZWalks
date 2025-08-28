using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.CustomActionFilters;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;
using System.Text.Json;

namespace NZWalks_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IMapper mapper, IImageRepository imageRepository, ILogger<ImagesController> logger)
        {
            this._mapper = mapper;
            this._imageRepository = imageRepository;
            this._logger = logger;
        }
        // POST: /api/Images/Upload
        [HttpPost("Upload")]
        [Authorize(Roles = ("Writer"))]
        [ValidateModel]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
            _logger.LogInformation("API call to upload image with filename: {FileName}, size: {FileSize} bytes", 
                imageUploadRequestDto?.File?.FileName, imageUploadRequestDto?.File?.Length);

            ValidateFileUpload(imageUploadRequestDto);

            if (!ModelState.IsValid) 
            {
                _logger.LogWarning("Image upload validation failed for file: {FileName}. Errors: {ValidationErrors}", 
                    imageUploadRequestDto?.File?.FileName, 
                    string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            // Map/Convert The DTO to Domain Model
            var image = _mapper.Map<Image>(imageUploadRequestDto);

            // Use repository to upload image
            await _imageRepository.Upload(image);

            _logger.LogInformation("Image uploaded successfully with filename: {FileName}, ID: {ImageId}", 
                imageUploadRequestDto.File.FileName, image.Id);

            return Ok(image);
        }

        private void ValidateFileUpload(ImageUploadRequestDto file)
        {
            _logger.LogDebug("Validating file upload for: {FileName}", file?.File?.FileName);

            string[] fileExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!fileExtensions.Contains(Path.GetExtension(file!.File.FileName)))
            {
                _logger.LogWarning("Invalid file extension for upload: {FileName}, Extension: {Extension}", 
                    file.File.FileName, Path.GetExtension(file.File.FileName));
                ModelState.AddModelError("file", "Unspported file extension.");
            }
            
            if (file.File.Length > 10485760) // 10MB
            {
                _logger.LogWarning("File size too large for upload: {FileName}, Size: {FileSize} bytes", 
                    file.File.FileName, file.File.Length);
                ModelState.AddModelError("file", "File size more than 10MB.");
            }

            _logger.LogDebug("File validation completed for: {FileName}", file?.File?.FileName);
        }
    }
}
