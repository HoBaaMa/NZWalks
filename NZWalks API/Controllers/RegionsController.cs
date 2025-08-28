using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks_API.CustomActionFilters;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;
using System.Text.Json;


namespace NZWalks_API.Controllers
{
    // https://localhost:portnumber/api/Regions
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            this._regionRepository = regionRepository;
            this._mapper = mapper;
            this._logger = logger;
        }

        // GET: https://localhost:portnumber/api/Regions
        [HttpGet]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("API call to get all regions");
            // Get Data From Database - Domain Models
            var regions = await _regionRepository.GetAllAsync();

            _logger.LogInformation("Finished get all regions with {RegionCount} regions retrieved", regions.Count());
            // Mapping Domain Models & Return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regions));
        }

        // GET: https://localhost:portnumber/api/Regions/{id}
        [HttpGet("{id:Guid}")]
        [Authorize(Roles = "Reader, Writer")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            _logger.LogInformation("API call to get region by ID: {RegionId}", id);

            // Get Region Domain Model From Database
            var region = await _regionRepository.GetByIdAsync(id);
            if (region is null)
            {
                _logger.LogWarning("Region not found with ID: {RegionId}", id);
                return NotFound();
            }

            _logger.LogInformation("Region retrieved successfully with ID: {RegionId}", id);
            // Mapping Domain Model & Return DTO
            return Ok(_mapper.Map<RegionDto>(region));
        }

        // POST: https://localhost:portnumber/api/Regions/
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            _logger.LogInformation("API call to create a new region with data: {RegionData}", JsonSerializer.Serialize(addRegionRequestDto));

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            // Check if The Input Data is Null
            if (addRegionRequestDto is null)
            {
                _logger.LogWarning("Create region request failed - null data provided");
                return BadRequest("Invalid data.");
            }

            // Map/Convert DTO to Domain Model
            var region = _mapper.Map<Region>(addRegionRequestDto);

            // Add & Save Domain Model Entity in The Database
            region = await _regionRepository.CreateAsync(region);

            _logger.LogInformation("Region created successfully with ID: {RegionId}", region.Id);

            // Map Domain Model Back to DTO
            var regionDto = _mapper.Map<RegionDto>(region);

            // Return Created Response with 201 Status Code. Passing The URI of the OR The Action Method & Anonymous Type to Pass The Region ID & The Region Entity (Domain Model)
            return CreatedAtAction(nameof(GetRegionById), new {id = regionDto.Id}, regionDto);
        }

        // PUT: https://localhost:portnumber/api/Regions/{id}
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            _logger.LogInformation("API call to update region with ID: {RegionId}, Data: {UpdateData}", id, JsonSerializer.Serialize(updateRegionRequestDto));

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            // Check if The Input Data is Null
            if (updateRegionRequestDto is null)
            {
                _logger.LogWarning("Update region request failed - null data provided for ID: {RegionId}", id);
                return BadRequest("Invalid data.");
            }

            // Map/Convert DTO to Domain Model
            var updateRegion = _mapper.Map<Region>(updateRegionRequestDto);

            // Get Region Domain Model From Database
            var region = await _regionRepository.UpdateAsync(id, updateRegion);

            // If The Region is Null; return NotFound() With Status Code 404
            if (region is null)
            {
                _logger.LogWarning("Region update failed - region not found with ID: {RegionId}", id);
                return NotFound();
            }

            _logger.LogInformation("Region updated successfully with ID: {RegionId}", id);
            // Return Ok With Status Code 200 & Mapping The Domain Model to DTO & Passing it
            return Ok(_mapper.Map<RegionDto>(region));
        }

        // DELETE: https://localhost:portnumber/api/Regions/{id}
        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation("API call to delete region with ID: {RegionId}", id);

            try
            {
                // Delete the region using repository
                await _regionRepository.DeleteAsync(id);

                _logger.LogInformation("Region deleted successfully with ID: {RegionId}", id);
                // Return NoContent With Status Code 204
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle case where the entity was already deleted or doesn't exist
                _logger.LogWarning(ex, "Region deletion failed - concurrency issue for ID: {RegionId}", id);
                return NotFound();
            }
        }
    }
}