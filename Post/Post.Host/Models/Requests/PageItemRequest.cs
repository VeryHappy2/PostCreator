using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests
{
    public class PageItemRequest
    {
        [Required]
        public int PageIndex { get; set; }
        [Required]
        public int PageSize { get; set; }
        public string? SearchByUserName { get; set; }
        public int? CategoryFilter { get; set; }
        [MaxLength(50)]
        public string? SearchByTitle { get; set; }
    }
}