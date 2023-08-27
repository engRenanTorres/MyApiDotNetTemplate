using System.ComponentModel;
using DotnetAPI.Data.Repositories;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Services
{
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
    public Task<Question> CreateQuestion(CreateQuestionDTO questionDTO)
    {
      throw new NotImplementedException();
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

    public Task<Question> GetQuestion(int id)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<Question>> GetQuestions()
    {
      throw new NotImplementedException();
    }

    public Task<Question> PatchQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO)
    {
      throw new NotImplementedException();
    }
  }
}