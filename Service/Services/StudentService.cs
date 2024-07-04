using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.GroupStudents;
using Service.DTOs.Admin.Students;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IGroupStudentRepository _groupStudentRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepo,
                              IGroupStudentRepository groupStudentRepo,
                              IMapper mapper,
                              IGroupRepository groupRepo)
        {
            _studentRepo = studentRepo;
            _groupStudentRepo = groupStudentRepo;
            _mapper = mapper;
            _groupRepo = groupRepo;
        }

        public async Task CreateAsync(StudentCreateDto model)
        {
            var data = _mapper.Map<Student>(model);
            await _studentRepo.CreateAsync(data);

            foreach (var id in model.GroupIds)
            {
                await _groupStudentRepo.CreateAsync(new GroupStudents
                {
                    StudentId = data.Id,
                    GroupId = id
                });
            }
        }

        public async Task EditAsync(int? id, StudentEditDto model)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            ArgumentNullException.ThrowIfNull(nameof(model));

            var student = await _studentRepo.GetById((int)id) ?? throw new NotFoundException("Data not found");

            _mapper.Map(model, student);
            await _studentRepo.EditAsync(student);
        }

        public async Task DeleteAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var student = await _studentRepo.GetById((int)id) ?? throw new NotFoundException("Data not found");

            await _studentRepo.DeleteAsync(student);
        }

        public async Task<IEnumerable<StudentDto>> GetAllWithInclude()
        {
            var students = await _studentRepo.FindAllWithIncludes()
                 .Include(m => m.GroupStudents)
                 .ThenInclude(m => m.Group)
                 .ToListAsync();
            var mappedStudents = _mapper.Map<List<StudentDto>>(students);
            return mappedStudents;
        }

        public async Task<StudentDto> GetByIdAsync(int? id)
        {
            var student = await _studentRepo.FindAllWithIncludes()
                .Where(m => m.Id == id)
                .Include(m => m.GroupStudents)
                .ThenInclude(m => m.Group)
                .FirstOrDefaultAsync();

            return _mapper.Map<StudentDto>(student);
        }

        public async Task AddGroupAsync(GroupStudentCreateDto model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (await _studentRepo.GetById(model.StudentId) is null)
            {
                throw new NotFoundException("Student not found");
            }

            var group = await _groupRepo.FindBy(m => m.Id == model.GroupId, m => m.GroupStudents).FirstOrDefaultAsync() ?? throw new NotFoundException("Group not found");

            if (group.GroupStudents.Count >= group.Capacity)
            {
                throw new BadRequestException("Group is full");
            }

            var groupTeacher = await _groupStudentRepo.FindBy(m => m.StudentId == model.StudentId && m.GroupId == model.GroupId).FirstOrDefaultAsync();

            if (groupTeacher is not null) throw new BadRequestException("Student is already in this group");

            await _groupStudentRepo.CreateAsync(_mapper.Map<GroupStudents>(model));
        }

        public async Task DeleteGroupAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var groupStudent = await _groupStudentRepo.GetById((int)id) ?? throw new NotFoundException("Data not found");

            await _groupStudentRepo.DeleteAsync(groupStudent);
        }
    }
}
