namespace Catalog.Host.Models.Requests
{
    public class PageItemRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int? CategoryFilter { get; set; }
        public string? Search { get; set; }
    }
}