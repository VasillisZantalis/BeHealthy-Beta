using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : base(contextFactory)
    {
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync()
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Rooms
            .Include(i => i.Department)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetRoomAppointmentsAsync(int roomId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Appointments
            .Where(w => w.RoomId == roomId)
            .ToListAsync();
    }

    public async Task<Room?> GetRoomByIdAsync(int roomId)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Rooms
            .Include(i => i.Department)
            .FirstOrDefaultAsync(w => w.Id == roomId);
    }
}
