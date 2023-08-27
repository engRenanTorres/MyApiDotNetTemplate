using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
  public class DataContextEF : DbContext
  {

    public DataContextEF(DbContextOptions<DataContextEF> options) : base(options)
    {
      // _conectionString = config.GetConnectionString("DefaultConnection");
    }
    public DbSet<User>? Users { get; set; }
    public DbSet<Question> Questions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<User>()
        .HasMany(q => q.Questions)
        .WithOne(u => u.CreatedBy)
        .IsRequired();
      /*modelBuilder.Entity<Question>()
        .HasOne(q => q.CreatedBy)
        .WithMany(u => u.Questions)
        .HasForeignKey(q => q.CreatedById)
        .IsRequired();*/
    }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder
        .Entity<User>()//.ToTable("User").HasKey(q => q.Id);
        .HasMany(u => u.Questions)
        .WithOne(q => q.CreatedBy)
        .HasForeignKey(q => q.CreatedById)
        .HasPrincipalKey(u => u.Id);
      modelBuilder
        .Entity<Question>()
        .ToTable("Questions")//.HasKey(q => q.Id);
        .HasOne(q => q.CreatedBy)
        .WithMany(u => u.Questions)
        .HasForeignKey(u => u.Id)
        .HasPrincipalKey(q => q.Id);
    }*/
  }
}