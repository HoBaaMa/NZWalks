using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;


namespace NZWalks_API.Controllers
{
    // https://localhost:portnumber/api/Regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this._regionRepository = regionRepository;
            this._mapper = mapper;
        }

        // GET: https://localhost:portnumber/api/Regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain Models
            var regions = await _regionRepository.GetAllAsync();

            // Mapping Domain Models & Return DTOs
            return Ok(_mapper.Map<List<RegionDto>>(regions));
        }

        // GET: https://localhost:portnumber/api/Regions/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetRegionById([FromRoute] Guid id)
        {
            // Get Region Domain Model From Database
            var region = await _regionRepository.GetByIdAsync(id);
            if (region is null)
            {
                return NotFound();
            }

            // Mapping Domain Model & Return DTO
            return Ok(_mapper.Map<RegionDto>(region));
        }

        // POST: https://localhost:portnumber/api/Regions/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Check if The Input Data is Null
            if (addRegionRequestDto is null)
            {
                return BadRequest("Invalid data.");
            }

            // Map/Convert DTO to Domain Model
            var region = _mapper.Map<Region>(addRegionRequestDto);

            // Add & Save Domain Model Entity in The Database
            region = await _regionRepository.CreateAsync(region);

            // Map Domain Model Back to DTO
            var regionDto = _mapper.Map<RegionDto>(region);

            // Return Created Response with 201 Status Code. Passing The URI of the OR The Action Method & Anonymous Type to Pass The Region ID & The Region Entity (Domain Model)
            return CreatedAtAction(nameof(GetRegionById), new {id = regionDto.Id}, regionDto);
        }

        // PUT: https://localhost:portnumber/api/Regions/{id}
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Check if The Input Data is Null
            if (updateRegionRequestDto is null)
            {
                return BadRequest("Invalid data.");
            }

            // Map/Convert DTO to Domain Model
            var updateRegion = _mapper.Map<Region>(updateRegionRequestDto);

            // Get Region Domain Model From Database
            var region = await _regionRepository.UpdateAsync(id, updateRegion);

            // If The Region is Null; return NotFound() With Status Code 404
            if (region is null)
            {
                return NotFound();
            }

            // Return Ok With Status Code 200 & Mapping The Domain Model to DTO & Passing it
            return Ok(_mapper.Map<RegionDto>(region));
        }

        // DELETE: https://localhost:portnumber/api/Regions/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                // Delete the region using repository
                await _regionRepository.DeleteAsync(id);

                // Return NoContent With Status Code 204
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle case where the entity was already deleted or doesn't exist
                return NotFound();
            }
        }
    }
}