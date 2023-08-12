using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data.Repositories;

class UserRepository : IUserRepository
{
  private readonly DataContextEF _context;
  public UserRepository(DataContextEF context)
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

  public async Task<IEnumerable<User?>> GetAllUsers()
  {
    if (_context.Users != null)
    {
      IEnumerable<User?> users = await _context.Users.ToListAsync();

      return users;
    }
    throw new Exception("Users repo is not set");
  }
  public async Task<User?> GetSingleUser(int id)
  {
    if (_context.Users != null)
    {
      User? user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
      return user;
    }
    throw new Exception("Users repo is not set");
  }
}