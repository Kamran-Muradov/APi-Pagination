using Service.DTOs.Admin.GroupStudents;
using Service.DTOs.Admin.Students;

namespace Service.Services.Interfaces
{
    public interface IStudentService
    {
        Task CreateAsync(StudentCreateDto model);
        Task EditAsync(int? id, StudentEditDto model);
        Task DeleteAsync(int? id);
        Task<IEnumerable<StudentDto>> GetAllWithInclude();
        Task<StudentDto> GetByIdAsync(int? id);
        Task AddGroupAsync(GroupStudentCreateDto model);
        Task DeleteGroupAsync(int? id);
    }
}
