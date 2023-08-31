using System.Text;
using System.Text.Json.Serialization;
using DotnetAPI.Data;
using DotnetAPI.Data.Repositories;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("ElevatedRights", policy =>
        policy.RequireRole("Administrator", "PowerUser", "BackupAdministrator"));
  options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("Role", "Adm")
    );
});*/

builder.Services.AddControllers()
  .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Scheme = "bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Name = "Authorization",
    Description = "Bearer Authentication with JWT Token",
    Type = SecuritySchemeType.Http
  });
  options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

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
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAuthService, AuthService>();

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters()
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
          tokenKeyString ?? ""
        )),
        ValidateIssuer = false,
        ValidateAudience = false
      };
    });


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


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
