using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks_API.CustomActionFilters;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;
using NZWalks_API.Repositories;

namespace NZWalks_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this._mapper = mapper;
            this._walkRepository = walkRepository;
        }


        // POST: https://localhost:portnumber/api/Walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            // Map DTO to Domain Model
            if (addWalkRequestDto is null)
            {
                return BadRequest("Invalid data");
            }

            var walk = _mapper.Map<Walk>(addWalkRequestDto);
            await _walkRepository.CreateAsync(walk);

            // Map Domain Model to DTO
            return Ok(_mapper.Map<WalkDto>(walk));
        }

        // GET: https://localhost:portnumber/api/Walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walks = await _walkRepository.GetAllAsync();

            // Map Domain Model to DTO
            return Ok(_mapper.Map<List<WalkDto>>(walks));
        }

        // GET: https://localhost:portnumber/api/Walks/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walk = await _walkRepository.GetByIdAsync(id);

            if (walk is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walk));
        }

        // PUT: https://localhost:portnumber/api/Walks/{id}
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            if (updateWalkRequestDto is null)
            {
                return BadRequest("Invalid data.");
            }

            // Map DTO to Domain Model
            var updateWalk = _mapper.Map<Walk>(updateWalkRequestDto);

            updateWalk = await _walkRepository.UpdateAsync(id, updateWalk);
            if (updateWalk is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(updateWalk));
        }

        // DELETE: https://localhost:portnumber/api/Walks/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _walkRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
    }
}
