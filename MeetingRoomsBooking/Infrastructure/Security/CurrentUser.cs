using MeetingRoomsBooking.Features.Abstractions.Security;

namespace MeetingRoomsBooking.Infrastructure.Security
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpCtxAccessor;

        public CurrentUser(IHttpContextAccessor accessor) => _httpCtxAccessor = accessor;


        public Guid EmployeeId
        {
            get
            {
                var value = _httpCtxAccessor.HttpContext?.Request.Headers["X-EmployeeID"]
                    .FirstOrDefault();

                if (!Guid.TryParse(value, out var id))
                    throw new UnauthorizedAccessException("Missing or invalid X-EmployeeID header.");

                return id;
            }
        }


        public UserRole Role
        {
            get
            {
                var value = _httpCtxAccessor.HttpContext?.Request.Headers["X-UserRole"]
                    .FirstOrDefault();

                if (!Enum.TryParse<UserRole>(value, ignoreCase: true, out var role))
                    throw new UnauthorizedAccessException("Missing or invalid X-UserRole header.");

                return role;
            }
        }
    }
}
