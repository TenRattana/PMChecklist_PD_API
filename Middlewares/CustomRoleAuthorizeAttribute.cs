using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

public class CustomRoleAuthorizeAttribute : ActionFilterAttribute
{
    private readonly string[] _requiredRoles;

    public CustomRoleAuthorizeAttribute(params string[] roles)
    {
        _requiredRoles = roles;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var authorizationHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();

        var userRolesClaim = context.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == "Permissions")?.Value;

        if (string.IsNullOrEmpty(userRolesClaim))
        {
            context.Result = new ForbidResult();
            return;
        }

        var userRoles = userRolesClaim.Split(',');

        if (!_requiredRoles.Any(role => userRoles.Contains(role)))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        base.OnActionExecuting(context);
    }
}
