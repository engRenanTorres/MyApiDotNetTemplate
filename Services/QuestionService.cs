using System.ComponentModel;
using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Services;

public class QuestionService : IQuestionService
{
  private readonly ILogger<QuestionService> _logger;
  private readonly IQuestionRepository _questionRepository;
  private readonly IUserRepository _userRepository;

  public QuestionService(
    ILogger<QuestionService> logger,
    IQuestionRepository repository,
    IUserRepository userRepository
  )
  {
    _logger = logger;
    _questionRepository = repository;
    _userRepository = userRepository;
  }
  async public Task<Question?> CreateQuestion(CreateQuestionDTO questionDTO, string userId)
  {
    _logger.LogInformation("CreateQuestion has been called.");

    User? user =
      await _userRepository.GetSingleUser(int.Parse(userId))
      ?? throw new WarningException("Not found user to create the question");

    Question question = new()
    {
      Body = questionDTO.Body,
      Answer = questionDTO.Answer.ToCharArray()[0],
      Tip = questionDTO.Tip,
      CreatedAt = DateTime.UtcNow,
      LastUpdatedAt = DateTime.UtcNow,
      CreatedBy = user,
    };


    _questionRepository.AddEntity(question);
    if (await _questionRepository.SaveChanges())
    {
      return question;
    }
    return null;
  }

  async public Task<bool> DeleteQuestion(int id)
  {
    _logger.LogInformation("Delete Question has been called.");

    Question question =
      await _questionRepository.GetSingleQuestion(id)
      ?? throw new WarningException("Question id: " + id + "not found");

    _questionRepository.RemoveEntity<Question>(question);
    return await _questionRepository.SaveChanges();
  }

  async public Task<Question?> GetQuestion(int id)
  {
    _logger.LogInformation("Get One Question Service has been called.");

    Question? question = await _questionRepository.GetSingleQuestion(id);

    _logger.LogInformation("Get One Question Service has finish succefully.");
    return question;
  }

  async public Task<IEnumerable<Question?>> GetAllQuestions()
  {
    _logger.LogInformation("Get Questions Service has been called.");
    IEnumerable<Question?> questions = await _questionRepository.GetAllQuestions();
    _logger.LogInformation("Get Questions Service has finish succefully.");
    return questions;
  }

  async public Task<Question> PatchQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO)
  {
    _logger.LogInformation("Patch QuestionService has been called.");
    Question question =
      await _questionRepository.GetSingleQuestion(id)
      ?? throw new WarningException("Question id: " + id + "not found");

    if (updateQuestionDTO.Body != null) question.Body = updateQuestionDTO.Body;
    if (updateQuestionDTO.Answer != null) question.Answer = (char)updateQuestionDTO.Answer;
    if (updateQuestionDTO.Tip != null) question.Tip = updateQuestionDTO.Tip;
    question.LastUpdatedAt = DateTime.UtcNow;

    if (await _questionRepository.SaveChanges())
    {
      _logger.LogInformation("Patch QuestionService has updated question successfully.");
      return question;
    }
    throw new Exception("Error to update Question");
  }
}
