using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Services
{
  public interface IQuestionService
  {
    Task<Question?> CreateQuestion(CreateQuestionDTO questionDTO, string userId);
    Task<Question?> GetQuestion(int id);
    Task<IEnumerable<Question?>> GetAllQuestions();
    Task<Question> PatchQuestion(int id, [FromBody] UpdateQuestionDTO updateQuestionDTO);
    Task<bool> DeleteQuestion(int id);
  }
}