using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {
        //private readonly IConfiguration _config;
        //private readonly string _conectionString = "";

    public DataContextEF(DbContextOptions<DataContextEF> options): base(options)
    {
        // _conectionString = config.GetConnectionString("DefaultConnection");
    }
    public DbSet<User>? Users {get; set;}
    public DbSet<Question> Questions{get; set;}
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if(!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseNpgsql(_conectionString,
        (options)=> options.EnableRetryOnFailure());
      }
    }*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>().ToTable("User").HasKey(u => u.Id);
      modelBuilder.Entity<Question>().ToTable("Questions").HasKey(u => u.Id);
    }
  }
}