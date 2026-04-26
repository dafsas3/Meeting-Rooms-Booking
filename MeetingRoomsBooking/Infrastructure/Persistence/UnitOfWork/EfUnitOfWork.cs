using MeetingRoomsBooking.Features.Abstractions.Persistence.UnitOfWork;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace MeetingRoomsBooking.Infrastructure.Persistence.UnitOfWork
{
    public sealed class EfUnitOfWork : IUnitOfWork
    {
        private readonly BookingDbContext _db;
        private IDbContextTransaction? _currentTransaction;

        public EfUnitOfWork(BookingDbContext db) => _db = db;

        public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

        public void ClearTracking() => _db.ChangeTracker.Clear();

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct)
        {
            _currentTransaction = await _db.Database.BeginTransactionAsync(ct);
            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(CancellationToken ct)
        {
            if (_currentTransaction is null) return;
            await _currentTransaction.CommitAsync(ct);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken ct)
        {
            if (_currentTransaction is null) return;
            await _currentTransaction.RollbackAsync(ct);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
