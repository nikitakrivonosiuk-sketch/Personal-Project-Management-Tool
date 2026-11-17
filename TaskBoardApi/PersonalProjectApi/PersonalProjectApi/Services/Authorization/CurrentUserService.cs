using System.Security.Claims;

namespace PersonalProjectApi.Services.Authorization
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userIdValue = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(userIdValue, out var userId))
                {
                    throw new InvalidOperationException("The authenticated user has no valid user ID claim.");
                }

                return userId;
            }
        }
    }
}
