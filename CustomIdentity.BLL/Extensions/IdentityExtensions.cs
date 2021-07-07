using System;
using System.Linq;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CustomIdentity.BLL.Extensions
{
    public static class IdentityExtensions
    {
        public static void ThrowExceptionOnFailure(this IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errorsDescription = result.Errors.Select(_ => _.Description);
                var errors = string.Join(Environment.NewLine, errorsDescription);
                throw new Exception(errors);
            }
        }

        public static void ThrowExceptionOnFailure(this SignInResult result)
        {
            if (!result.Succeeded)
            {
                throw new AuthenticationException();
            }
        }
    }
}
