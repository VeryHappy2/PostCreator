using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class PageItemRequest
    {
        [Required(ErrorMessage = "Page index is required")]
        public int PageIndex { get; set; }
        [Required(ErrorMessage = "Page size is required")]
        public int PageSize { get; set; }
        public string? SearchByUserName { get; set; }
        public int? CategoryFilter { get; set; }
        [MaxLength(50, ErrorMessage = @"The field ""Searching by title"" can has only 50 symbols")]
        public string? SearchByTitle { get; set; }
    }
}