using System.ComponentModel.DataAnnotations;
using BlogApi.Models.Enums;

namespace BlogApi.Models.DTO;
public class QueryParametersPost { 
    
    public List<Guid>? Tags { get; set; }
    public string? AuthorsName { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "The field must be a non-negative integer.")]
    public int MinReadingTime { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "The field must be a non-negative integer.")]
    public int MaxReadingTime { get; set; }
    public PostSorting? Sorting { get; set; }
    public bool OnlyMyCommunities { get; set; } = false;
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 5;
}
