
using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionController : ControllerBase
{
  private readonly ILogger<QuestionController> _logger;
  private readonly IQuestionRepository _questionRepository;
  // TODO remove and use userService
  private readonly IUserRepository _userRepository;

  public QuestionController(ILogger<QuestionController> logger, IQuestionRepository repository, IUserRepository userRepository)
  {
    _logger = logger;
    _questionRepository = repository;
    _userRepository = userRepository;
  }
  [HttpPost("")]
  public async Task<ActionResult<Question>> CreateQuestion(CreateQuestionDTO questionDTO)
  {
    _logger.LogInformation("CreateQuestion has been called.");

    if (questionDTO == null) return BadRequest("Question data is null.");
    string? userId = User?.FindFirst("userId")?.Value;

    if (userId == null) return BadRequest("User id: " + userId + "not found");

    User? user = await _userRepository.GetSingleUser(int.Parse(userId));
    if (user == null) return NotFound("Not found user to create the question");
    Console.WriteLine($"----------- aqui ========= {user.Name}");

    Question question = new()
    {
      Body = questionDTO.Body,
      Answer = questionDTO.Answer,
      Tip = questionDTO.Tip,
      CreatedAt = DateTime.UtcNow,
      LastUpdatedAt = DateTime.UtcNow,
      CreatedBy = user,
    };


    _questionRepository.AddEntity(question);
    // _constext.SaveChanges return de number of rows that were modified.
    if (await _questionRepository.SaveChanges())
    {
      return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
    }
    throw new Exception("Error to Add this Question");

  }
  [AllowAnonymous]
  [HttpGet("{id}")]
  public async Task<ActionResult<Question>> GetQuestion(int id)
  {
    _logger.LogInformation("GetQuestions has been called.");

    Question? question = await _questionRepository.GetSingleQuestion(id);

    return Ok(question);
  }

  [AllowAnonymous]
  [HttpGet("")]
  public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
  {
    _logger.LogInformation("GetQuestions has been called.");
    IEnumerable<Question?> questions = await _questionRepository.GetAllQuestions();

    return Ok(questions);
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<Question>> PatchQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO)
  {
    _logger.LogInformation("PatchQuestions has been called.");
    Question? question = await _questionRepository.GetSingleQuestion(id);
    if (question == null)
      if (question == null)
      {
        return NotFound("Question id: " + id + "not found");
      }
    if (updateQuestionDTO.Body != null) question.Body = updateQuestionDTO.Body;
    if (updateQuestionDTO.Answer != null) question.Answer = (char)updateQuestionDTO.Answer;
    if (updateQuestionDTO.Tip != null) question.Tip = updateQuestionDTO.Tip;
    question.LastUpdatedAt = DateTime.UtcNow;

    if (await _questionRepository.SaveChanges())
    {
      return Ok(question);
    }
    throw new Exception("Error to update Question");
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteQuestion(int id)
  {
    _logger.LogInformation("DeleteQuestion has been called.");

    Question? question = await _questionRepository.GetSingleQuestion(id);

    if (question == null)
    {
      return NotFound("Question id: " + id + "not found");
    }

    _questionRepository.RemoveEntity<Question>(question);
    if (await _questionRepository.SaveChanges())
    {
      return NoContent();
    };
    throw new Exception("Error to delete Question");


  }
}
/*
  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteQuestion(int id)
  {
    _logger.LogInformation("Delete Question Controller has been called.");

    try
    {
      var deleteQuestion = await _questionService.DeleteQuestion(id);
      if (deleteQuestion)
      {
        return NoContent();
      };
    }
    catch (Exception ex)
    {
      if (ex is WarningException)
      {
        _logger.LogError(ex, "Question not found while deleting.");
        return NotFound("Question id: " + id + "not found");
      }
      _logger.LogError(ex, "An error occurred while deleting the question.");
    }
    throw new Exception("Error to delete Question");
  }
*/
