using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Structure.JWT
{
    public class CheckAdminAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                var isAdminClaim = user.Claims.FirstOrDefault(c => c.Type == "isAdmin");
                if (isAdminClaim == null || !bool.TryParse(isAdminClaim.Value, out bool isAdmin) || !isAdmin)
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
