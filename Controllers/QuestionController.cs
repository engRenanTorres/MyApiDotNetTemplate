using DotnetAPI.Data;
using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
  private readonly ILogger<QuestionController> _logger;
  private readonly IQuestionRepository _questionRepository;

  public QuestionController(ILogger<QuestionController> logger, IQuestionRepository repository)
  {
    _logger = logger;
    _questionRepository = repository;
  }
  [HttpPost("")]
  public async Task<ActionResult<Question>> CreateQuestion(CreateQuestionDTO questionDTO)
  {
    _logger.LogInformation("CreateQuestion has been called.");

    if (questionDTO == null)
    {
      return BadRequest("Question data is null.");
    }

    Question question = new()
    {
      Body = questionDTO.Body,
      Answer = questionDTO.Answer,
      Tip = questionDTO.Tip,
      CreatedAt = DateTime.UtcNow
    };


    _questionRepository.AddEntity(question);
    // _constext.SaveChanges return de number of rows that were modified.
    if (await _questionRepository.SaveChanges())
    {
      return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
    }
    throw new Exception("Error to Add this Question");

  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Question>> GetQuestion(int id)
  {
    _logger.LogInformation("GetQuestions has been called.");

    Question? question = await _questionRepository.GetSingleQuestion(id);

    return Ok(question);
  }

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
