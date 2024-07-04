using Service.DTOs.Admin.Rooms;

namespace Service.Services.Interfaces
{
    public interface IRoomService
    {
        Task CreateAsync(RoomCreateDto model);
        Task EditAsync(int? id, RoomEditDto  model);
        Task DeleteAsync(int? id);
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<RoomDto> GetByIdAsync(int? id);
    }
}
