using Managers;
using System.Security.Claims;

namespace Clients.Web.Extensions
{

    public static class ClaimsPrincipalExtensions
    {
        public static UserId GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            return new UserId(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
    
}
