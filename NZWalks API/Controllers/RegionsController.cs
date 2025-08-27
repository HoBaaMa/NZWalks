using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NZWalks_API.Data;
using NZWalks_API.Models.Domain;
using NZWalks_API.Models.DTOs;
using static System.Net.WebRequestMethods;

namespace NZWalks_API.Controllers
{
    // https://localhost:portnumber/api/Regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _context;
        public RegionsController(NZWalksDbContext context)
        {
            this._context = context;
        }

        // GET: https://localhost:portnumber/api/Regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //var regions = new List<Region>
            //{
            //    new Region {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland Region",
            //        Code = "AKL",
            //        ImageUrl = @"https://www.pexels.com/photo/luxury-apartment-view-over-auckland-skyline-33634443/"
            //    },
            //    new Region {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington Region",
            //        Code = "WLG",
            //        ImageUrl = @"https://www.pexels.com/photo/photo-of-a-nimbus-clouds-during-sunset-1108234/"
            //    }
            //};

            // Get Data From Database - Domain Models
            var regions = _context.Regions.ToList();

            // Map Domain Models to DTOs

            var regionsDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    ImageUrl = region.ImageUrl
                });
            }

            // Return DTOs
            return Ok(regionsDto);
        }

        // GET: https://localhost:portnumber/api/Regions/{id}
        [HttpGet("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute] Guid id)
        {

            // var region = _context.Regions.Find(id);

            // Get Region Domain Model From Database
            var region = _context.Regions.FirstOrDefault(r => r.Id == id);
            if (region is null)
            {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO
            var regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                ImageUrl = region.ImageUrl
            };

            // Return DTO
            return Ok(regionDto);
        }

        // POST: https://localhost:portnumber/api/Regions/
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Check if The Input Data is Null
            if (addRegionRequestDto is null)
            {
                return BadRequest("Invalid data.");
            }

            // Map/Convert DTO to Domain Model
            var region = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                ImageUrl = addRegionRequestDto.ImageUrl
            };

            // Add Domain Model Entity in The Database
            _context.Regions.Add(region);

            // Save The Changes into The Database
            _context.SaveChanges();

            // Map Domain Model Back to DTO
            var regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                ImageUrl = region.ImageUrl
            };

            // Return Created Response with 201 Status Code. Passing The URI of the OR The Action Method & Anonymous Type to Pass The Region ID & The Region Entity (Domain Model)
            return CreatedAtAction(nameof(GetRegionById), new {id = regionDto.Id}, regionDto);
        }

        // PUT: https://localhost:portnumber/api/Regions/{id}
        [HttpPut("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Check if The Input Data is Null
            if (updateRegionRequestDto is null)
            {
                return BadRequest("Invalid data.");
            }

            // Get Region Domain Model From Database
            var region = _context.Regions.FirstOrDefault(r => r.Id == id);

            // If The Region is Null; return NotFound() With Status Code 404
            if (region is null)
            {
                return NotFound();
            }

            // Map/Convert DTO to Domain Model For Updating The Values
            region.Name = updateRegionRequestDto.Name;
            region.Code = updateRegionRequestDto.Code;
            region.ImageUrl = updateRegionRequestDto.ImageUrl;

            // Save Lastest Changes into Database
            _context.SaveChanges();


            // Convert Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                ImageUrl = region.ImageUrl
            };

            // Return Ok With Status Code 200 Passing The Region Details DTO 
            return Ok(regionDto);
        }

        // DELETE: https://localhost:portnumber/api/Regions/{id}
        [HttpDelete("{id:Guid}")]
        public IActionResult Delete(Guid id)
        {
            // Get Region Domain Model From Database
            var region = _context.Regions.FirstOrDefault(r => r.Id == id);

            // If The Region is Null; return NotFound() With Status Code 404
            if (region is null)
            {
                return NotFound();
            }

            // Remove The Region Entity From The Database
            _context.Regions.Remove(region);

            // Save The Changes 
            _context.SaveChanges();

            // Return NoContent With Status Code 204
            return NoContent();
        }
    }
}