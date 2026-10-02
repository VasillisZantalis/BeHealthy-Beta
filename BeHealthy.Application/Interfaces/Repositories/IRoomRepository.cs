using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<IEnumerable<Room>> GetAllRoomsAsync(CancellationToken cancellationToken = default);
    Task<Room?> GetRoomByIdAsync(int roomId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetRoomAppointmentsAsync(int roomId, CancellationToken cancellationToken = default);
}
