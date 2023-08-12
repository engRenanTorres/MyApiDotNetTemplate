using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data.Repositories;

class QuestionRepository : IQuestionRepository
{
  private readonly DataContextEF _context;
  public QuestionRepository(DataContextEF context)
  {
    _context = context;
  }

  public async Task<bool> SaveChanges()
  {
    return await _context.SaveChangesAsync() > 0;
  }

  public void AddEntity<T>(T entity)
  {
    if (entity != null)
    {
      _context.Add(entity);
    }
  }

  public void RemoveEntity<T>(T entity)
  {
    if (entity != null) { _context.Remove(entity); }
  }

  public async Task<IEnumerable<Question?>> GetAllQuestions()
  {
    if (_context.Questions != null)
    {
      IEnumerable<Question?> questions = await _context.Questions.ToListAsync();

      return questions;
    }
    throw new Exception("Questions repo is not set");
  }
  public async Task<Question?> GetSingleQuestion(int id)
  {
    if (_context.Questions != null)
    {
      Question? question = await _context.Questions.SingleOrDefaultAsync(u => u.Id == id);
      return question;
    }
    throw new Exception("Questions repo is not set");
  }
}