using BlogApi.Models.Entities;
using BlogApi.Models.Enums;

namespace BlogApi.Models.DTO
{
    public class QueryParametersCommunity
    {
        public List<Guid>? Tags { get; set; }

        public PostSorting? Sorting { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
