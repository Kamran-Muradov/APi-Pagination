using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Teachers;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepo;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepo = teacherRepository;
            _mapper = mapper;
            
        }

        public async Task Create(TeacherCreateDto model)
        {
            await _teacherRepo.CreateAsync(_mapper.Map<Teacher>(model));
        }

        public async Task EditAsync(int? id, TeacherEditDto model)
        {
            if (id is null) throw new ArgumentNullException();

            var teacher = await _teacherRepo.GetById((int)id);

            if (teacher is null) throw new NotFoundException("Data was not found");

            await _teacherRepo.EditAsync(_mapper.Map(model, teacher));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var teacher = await _teacherRepo.GetById((int)id);

            if (teacher is null) throw new NotFoundException("Data was not found");

            await _teacherRepo.DeleteAsync(teacher);
        }

        public async  Task<IEnumerable<TeacherDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<TeacherDto>>(await _teacherRepo.GetAllAsync());

        }

        public async Task<TeacherDto> GetByIdAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var teacher = await _teacherRepo.GetById((int)id);

            if (teacher is null) throw new NotFoundException("Data was not found");

            return _mapper.Map<TeacherDto>(teacher);
        }
    }
}
