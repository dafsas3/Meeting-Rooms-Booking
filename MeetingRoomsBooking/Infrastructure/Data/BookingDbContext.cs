using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Data
{
    public sealed class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : 
            base(options) { }

        public DbSet<Room> Rooms => Set<Room>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
