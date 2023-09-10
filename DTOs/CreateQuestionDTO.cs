using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.DTOs;
public class CreateQuestionDTO
{
    [Required]
    public string Body { get; set; } = "";
    [Required]
    [StringLength(1)]
    [RegularExpression(@"^[A|B|C|D|E|V|F]$", ErrorMessage = "Answer field accepts only the values A, B, C, D, E, V or F.")]
    public string Answer { get; set; } = "A";

    public string? Tip { get; set; }
}
