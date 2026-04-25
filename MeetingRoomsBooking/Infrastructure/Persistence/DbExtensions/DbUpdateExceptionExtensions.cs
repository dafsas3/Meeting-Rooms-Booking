using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Persistence.DbExtensions
{
    public static class DbUpdateExceptionExnetsions
    {
        public static bool TryGetDuplicateName(this DbUpdateException ex, out string? constraintName)
        {
            constraintName = null;

            if (ex.InnerException is not PostgresException pEx) return false;

            if(pEx.SqlState is not PostgresErrorCodes.UniqueViolation) return false;

            constraintName = pEx.ConstraintName;
            return true;
        }
    }
}
