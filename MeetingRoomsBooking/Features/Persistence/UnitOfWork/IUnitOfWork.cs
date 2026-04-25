using Microsoft.EntityFrameworkCore.Storage;

namespace MeetingRoomsBooking.Features.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct);
        void ClearTracking();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
        Task CommitTransactionAsync(CancellationToken ct);
        Task RollbackTransactionAsync(CancellationToken ct);
    }
}
