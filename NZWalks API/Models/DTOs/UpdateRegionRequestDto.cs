namespace NZWalks_API.Models.DTOs
{
    public class UpdateRegionRequestDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
