using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data
{
    public sealed class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) :
            base(options)
        { }

        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<BookingRequest> BookingRequests => Set<BookingRequest>();
        public DbSet<BookingHistory> BookingHistories => Set<BookingHistory>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
