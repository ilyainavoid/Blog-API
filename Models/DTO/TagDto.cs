﻿using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.DTO;

public class TagDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreateTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
}