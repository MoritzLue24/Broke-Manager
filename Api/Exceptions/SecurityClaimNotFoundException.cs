
using System.Security.Claims;

namespace Api.Exceptions
{
    public class SecurityClaimNotFoundException : Exception
    {
        /// <summary>
        /// Throws when a security claim is not found for some reason.
        /// </summary>
        /// <param name="claimType">The claim type, e.g. ClaimTypes.NameIdentifier</param>
        public SecurityClaimNotFoundException(string claimType)
            : base($"Security claim '{claimType}' not found") {}
    }
}