using DotnetAPI.Models;

namespace DotnetAPI.Data.Repositories;
public interface IUserRepository
{
  public Task<bool> SaveChanges();
  public void AddEntity<T>(T entity);
  public void RemoveEntity<T>(T entity);
  public Task<IEnumerable<User?>> GetAllUsers();
  public Task<User?> GetSingleUser(int id);
}