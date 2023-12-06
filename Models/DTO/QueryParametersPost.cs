using BlogApi.Models.Enums;

namespace BlogApi.Models.DTO;

public class QueryParametersPost
{
    public class QueryParametersCommunity
    {
        public List<Guid>? Tags { get; set; }
        public string? AuthorsName { get; set; }
        public int MinReadingTime { get; set; }
        public int MaxReadingTime { get; set; }
        public PostSorting? Sorting { get; set; }
        public bool OnlyMyCommunities { get; set; } = false;
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}