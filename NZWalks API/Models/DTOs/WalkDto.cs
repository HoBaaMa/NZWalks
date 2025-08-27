namespace NZWalks_API.Models.DTOs
{
    public class WalkDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? ImageUrl { get; set; }
        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
