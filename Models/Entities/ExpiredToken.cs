using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApi.Models.Entities;

public class ExpiredToken
{
    [Key]
    public string Token { get; set; }
}