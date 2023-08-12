using DotnetAPI.Models;

namespace DotnetAPI.Data.Repositories;
public interface IQuestionRepository
{
  public Task<bool> SaveChanges();
  public void AddEntity<T>(T entity);
  public void RemoveEntity<T>(T entity);
  public Task<IEnumerable<Question?>> GetAllQuestions();
  public Task<Question?> GetSingleQuestion(int id);
}