using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BlogApi.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommunityRole
{
    [EnumMember(Value = "Administrator")]
    Administrator,

    [EnumMember(Value = "Subscriber")]
    Subscriber
}