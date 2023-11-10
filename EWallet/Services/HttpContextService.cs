using EWallet.Services.Interfaces;
using System.Security.Claims;

namespace EWallet.Services
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid GetCurrentUserID()
        {
            var userIdClaim = _contextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim!);
        }
    }
}
