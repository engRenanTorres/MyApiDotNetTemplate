namespace DotnetAPI.DTOs;
public class CreateQuestionDTO
{
    public string Body { get; set; } = "";
    public char Answer { get; set; } = 'A';
    public string? Tip { get; set; }
}
