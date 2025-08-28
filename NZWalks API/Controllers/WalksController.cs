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
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;
        private readonly ILogger<WalksController> _logger;

        public WalksController(IMapper mapper, IWalkRepository walkRepository, ILogger<WalksController> logger)
        {
            this._mapper = mapper;
            this._walkRepository = walkRepository;
            this._logger = logger;
        }


        // POST: https://localhost:portnumber/api/Walks
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = ("Writer"))]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            _logger.LogInformation("API call to create a new walk with data: {WalkData}", JsonSerializer.Serialize(addWalkRequestDto));

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            // Map DTO to Domain Model
            if (addWalkRequestDto is null)
            {
                _logger.LogWarning("Create walk request failed - null data provided");
                return BadRequest("Invalid data");
            }

            var walk = _mapper.Map<Walk>(addWalkRequestDto);
            await _walkRepository.CreateAsync(walk);

            _logger.LogInformation("Walk created successfully with ID: {WalkId}", walk.Id);

            // Map Domain Model to DTO
            return Ok(_mapper.Map<WalkDto>(walk));
        }

        // GET: https://localhost:portnumber/api/Walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        [Authorize(Roles = ("Writer, Reader"))]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, int pageNumber = 1, int pageSize = 10)
        {
            _logger.LogInformation("API call to get all walks with filters - FilterOn: {FilterOn}, FilterQuery: {FilterQuery}, SortBy: {SortBy}, IsAscending: {IsAscending}, PageNumber: {PageNumber}, PageSize: {PageSize}", 
                filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            var walks = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            _logger.LogInformation("Retrieved {WalkCount} walks from database", walks.Count());

            // Map Domain Model to DTO
            return Ok(_mapper.Map<List<WalkDto>>(walks));
        }

        // GET: https://localhost:portnumber/api/Walks/{id}
        [HttpGet("{id:Guid}")]
        [Authorize(Roles = ("Writer, Reader"))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            _logger.LogInformation("API call to get walk by ID: {WalkId}", id);

            var walk = await _walkRepository.GetByIdAsync(id);

            if (walk is null)
            {
                _logger.LogWarning("Walk not found with ID: {WalkId}", id);
                return NotFound();
            }

            _logger.LogInformation("Walk retrieved successfully with ID: {WalkId}", id);
            return Ok(_mapper.Map<WalkDto>(walk));
        }

        // PUT: https://localhost:portnumber/api/Walks/{id}
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = ("Writer"))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            _logger.LogInformation("API call to update walk with ID: {WalkId}, Data: {UpdateData}", id, JsonSerializer.Serialize(updateWalkRequestDto));

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (updateWalkRequestDto is null)
            {
                _logger.LogWarning("Update walk request failed - null data provided for ID: {WalkId}", id);
                return BadRequest("Invalid data.");
            }

            // Map DTO to Domain Model
            var updateWalk = _mapper.Map<Walk>(updateWalkRequestDto);

            updateWalk = await _walkRepository.UpdateAsync(id, updateWalk);
            if (updateWalk is null)
            {
                _logger.LogWarning("Walk update failed - walk not found with ID: {WalkId}", id);
                return NotFound();
            }

            _logger.LogInformation("Walk updated successfully with ID: {WalkId}", id);
            return Ok(_mapper.Map<WalkDto>(updateWalk));
        }

        // DELETE: https://localhost:portnumber/api/Walks/{id}
        [HttpDelete("{id:Guid}")]
        [Authorize(Roles = ("Writer"))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            _logger.LogInformation("API call to delete walk with ID: {WalkId}", id);

            try
            {
                await _walkRepository.DeleteAsync(id);
                _logger.LogInformation("Walk deleted successfully with ID: {WalkId}", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogWarning(ex, "Walk deletion failed - concurrency issue for ID: {WalkId}", id);
                return NotFound();
            }
        }
    }
}
