namespace Plantt.Domain.QueryParams
{
    public class PlantQueryParams
    {
        // [FromQuery] int pagesize = 20, [FromQuery] bool detailed = false

        public int PageSize { get; set; } = 20;
        public bool Detailed { get; set; } = false;
        public string? Search { get; set; }
    }
}
