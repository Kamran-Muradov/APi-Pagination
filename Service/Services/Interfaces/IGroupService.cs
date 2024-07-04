using Service.DTOs.Admin.Groups;
using Service.DTOs.Admin.GroupTeachers;

namespace Service.Services.Interfaces
{
    public interface IGroupService
    {
        Task CreateAsync(GroupCreateDto model);
        Task EditAsync(int? id, GroupEditDto model);
        Task DeleteAsync(int? id);
        Task<IEnumerable<GroupDto>> GetAllAsync();
        Task<GroupDto> GetByIdAsync(int? id);
        Task AddTeacherAsync(GroupTeacherCreateDto  model);
        Task DeleteTeacherAsync(int? id);
    }
}
