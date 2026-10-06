using BeHealthy.Infrastructure.Data;
using BeHealthy.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BeHealthy.Application.Interfaces.Repositories;

namespace BeHealthy.Infrastructure.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Room>> GetAllRoomsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Rooms
            .Include(i => i.Department)
            .ToListAsync(cancellationToken);
    }

    public async Task<Room?> GetRoomByIdAsync(int roomId, CancellationToken cancellationToken = default)
    {
        return await _context.Rooms
            .Include(i => i.Department)
            .FirstOrDefaultAsync(w => w.Id == roomId, cancellationToken);
    }
}
