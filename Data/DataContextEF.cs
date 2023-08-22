using DotnetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
  public class DataContextEF : DbContext
  {
    //private readonly IConfiguration _config;
    //private readonly string _conectionString = "";

    public DataContextEF(DbContextOptions<DataContextEF> options) : base(options)
    {
      // _conectionString = config.GetConnectionString("DefaultConnection");
    }
    public DbSet<User>? Users { get; set; }
    public DbSet<Question> Questions { get; set; }
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
      modelBuilder
        .Entity<User>().ToTable("User");
      /*.HasKey(q => q.Id);
      .HasMany(u => u.Questions)
      .WithOne(q => q.CreatedBy)
      .HasForeignKey(u => u.Id)
      .IsRequired();*/
      modelBuilder
        .Entity<Question>()
        .ToTable("Questions").HasKey(q => q.Id);
      //.HasOne(q => q.CreatedBy).WithMany(u => u.Questions).HasForeignKey(u => u.Id).IsRequired();
    }
  }
}