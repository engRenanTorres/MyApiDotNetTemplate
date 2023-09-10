using DotnetAPI.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotnetAPI.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationLevel : Attribute, IAuthorizationFilter
{
  private readonly IList<string> _claimValue;

  public AuthorizationLevel(string claimName)
  {
    _claimValue = claimName.Split("|");
  }
  public void OnAuthorization(AuthorizationFilterContext context)
  {

    string? trampo = context.HttpContext.User?.FindFirst("trampo")?.Value;

    //Console.WriteLine(" ------------Jessica bocó " + trampo + " x");
    if (!_claimValue.Contains(context.HttpContext.User?.FindFirst("trampo")?.Value ?? "null"))
    {
      //Console.WriteLine(" ------------Jessica legal" + _claimValue[0] + " " + _claimValue.Count);
      context.Result = new ForbidResult();
    }
  }
}