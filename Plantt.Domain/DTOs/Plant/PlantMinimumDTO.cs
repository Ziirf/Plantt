namespace Plantt.Domain.DTOs.Plant
{
    public class PlantMinimumDTO
    {
        public required int Id { get; set; }
        public string? CommonName { get; set; }
        public required string LatinName { get; set; }
        public required string ImageUrl { get; set; }
    }
}
