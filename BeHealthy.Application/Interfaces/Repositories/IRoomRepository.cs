using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Interfaces.Repositories;

public interface IRoomRepository : IGenericRepository<Room>
{
    Task<IEnumerable<Room>> GetAllRoomsAsync();
    Task<Room?> GetRoomByIdAsync(int roomId);
    Task<List<Appointment>> GetRoomAppointmentsAsync(int roomId);
}
