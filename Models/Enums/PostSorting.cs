using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BlogApi.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PostSorting
{
    [EnumMember(Value = "CreateDesc")]
    CreateDesc,
    [EnumMember(Value = "CreateAsc")]
    CreateAsc,
    [EnumMember(Value = "LikeAsc")]
    LikeAsc,
    [EnumMember(Value = "LikeDesc")]
    LikeDesc
}