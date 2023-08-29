using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotnetAPI.Indentity;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationLevel : Attribute, IAuthorizationFilter
{
  private readonly string _claimValue;

  public AuthorizationLevel(string claimName)
  {
    _claimValue = claimName;
  }
  public void OnAuthorization(AuthorizationFilterContext context)
  {

    string? trampo = context.HttpContext.User?.FindFirst("trampo")?.Value;

    Console.WriteLine(" ------------Jessica bocó " + trampo + " x");
    if (context.HttpContext.User?.FindFirst("trampo")?.Value != "Jeca")
    {
      Console.WriteLine(" ------------Jessica legal");
      context.Result = new ForbidResult();
    }
  }
}