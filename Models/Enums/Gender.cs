using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BlogApi.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    [EnumMember(Value = "Male")]
    Male,

    [EnumMember(Value = "Female")]
    Female
}