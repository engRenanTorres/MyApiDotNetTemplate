using DotnetAPI;
using DotnetAPI.Data;
using DotnetAPI.Data.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

/*IConfiguration config = new ConfigurationBuilder()
  .AddJsonFile("appsettings.json")
  .Build();
DataContextEF dataContextEF= new(config);

User user = new(){
  Name = "Renan",
};


dataContextEF.Add(user);
dataContextEF.SaveChanges(); */

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
  .AddEntityFrameworkNpgsql()
  .AddDbContext<DataContextEF>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCors((options) =>
{
  options.AddPolicy("DevCors", (corsBuilder) =>
  {
    corsBuilder.WithOrigins("http://localhost:3000")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials();
  });
  options.AddPolicy("ProdCors", (corsBuilder) =>
  {
    corsBuilder.WithOrigins("https://engenhariadeconcursos.com.br")
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials();
  });
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseCors("DevCors");
}

if (app.Environment.IsProduction())
{
  app.UseHttpsRedirection();
  app.UseCors("ProdCors");
}


app.UseAuthorization();

app.MapControllers();

app.Run();
