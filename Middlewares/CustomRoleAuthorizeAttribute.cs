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

        var userPermissions = context.HttpContext.User.Claims
            .Where(c => c.Type == "Permissions")  
            .Select(c => c.Value)  
            .ToArray();  

        if (userPermissions == null || !userPermissions.Any())
        {
            context.Result = new ForbidResult();
            return;
        }

      bool allRequiredPermissionsPresent = _requiredRoles.All(role => userPermissions.Contains(role));

        if (!allRequiredPermissionsPresent)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        base.OnActionExecuting(context);
    }
}
