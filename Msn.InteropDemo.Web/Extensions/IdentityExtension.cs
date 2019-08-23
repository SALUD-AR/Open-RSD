using System.Security.Claims;

namespace Msn.InteropDemo.Web.Extensions
{
    public static class IdentityExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            #if DEBUG
            if (!user.Identity.IsAuthenticated)
            {
                return "1";
            }
            #endif

            var currentUser = user;
            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
