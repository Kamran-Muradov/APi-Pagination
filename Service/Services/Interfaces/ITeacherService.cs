using Service.DTOs.Admin.Teachers;

namespace Service.Services.Interfaces
{
    public interface ITeacherService
    {
        Task Create(TeacherCreateDto model);
        Task EditAsync(int? id, TeacherEditDto model);
        Task DeleteAsync(int? id);
        Task<IEnumerable<TeacherDto>> GetAll();
        Task<TeacherDto> GetByIdAsync(int? id);
    }
}
