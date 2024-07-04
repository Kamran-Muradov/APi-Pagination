using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Groups;
using Service.DTOs.Admin.GroupTeachers;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupTeacherRepository _groupTeacherRepository;
        private readonly IEducationRepository _educationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository,
                            IMapper mapper,
                            IGroupTeacherRepository groupTeacherRepository,
                            IEducationRepository educationRepository,
                            IRoomRepository roomRepository,
                            ITeacherRepository teacherRepository)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
            _groupTeacherRepository = groupTeacherRepository;
            _educationRepository = educationRepository;
            _roomRepository = roomRepository;
            _teacherRepository = teacherRepository;
        }

        public async Task CreateAsync(GroupCreateDto model)
        {
            var data = _mapper.Map<Group>(model);

            if (await _educationRepository.GetById(model.educationId) is null)
            {
                throw new NotFoundException("Education not found");
            }

            if (await _roomRepository.GetById(model.roomId) is null)
            {
                throw new NotFoundException("Room not found");
            }

            await _groupRepository.CreateAsync(data);
            await _groupTeacherRepository.CreateAsync(new GroupTeachers { GroupId = data.Id, TeacherId = model.teacherId });
        }

        public async Task DeleteAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var group = await _groupRepository.GetById((int)id) ?? throw new NotFoundException("Data not found");

            await _groupRepository.DeleteAsync(group);
        }

        public async Task EditAsync(int? id, GroupEditDto model)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var group = await _groupRepository.GetById((int)id) ?? throw new NotFoundException("Data not found");

            if (await _educationRepository.GetById(model.educationId) is null)
            {
                throw new NotFoundException("Education not found");
            }

            if (await _roomRepository.GetById(model.roomId) is null)
            {
                throw new NotFoundException("Room not found");
            }

            _mapper.Map(model, group);
            await _groupRepository.EditAsync(group);
        }

        public async Task<IEnumerable<GroupDto>> GetAllAsync()
        {
            var datas = await _groupRepository.FindAllWithIncludes()
                                              .Include(m => m.Education).Include(m => m.Room)
                                              .Include(m => m.GroupTeachers)
                                              .ThenInclude(m => m.Teacher)
                                              .Include(m => m.GroupStudents)
                                              .ThenInclude(m => m.Student).ToListAsync();

            return _mapper.Map<IEnumerable<GroupDto>>(datas);
        }

        public async Task<GroupDto> GetByIdAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var group = await _groupRepository.FindAllWithIncludes()
                .Where(m => m.Id == id)
                .Include(m => m.Education).Include(m => m.Room)
                .Include(m => m.GroupTeachers)
                .ThenInclude(m => m.Teacher)
                .Include(m => m.GroupStudents)
                .ThenInclude(m => m.Student)
                .FirstOrDefaultAsync();

            return group is null ? throw new NotFoundException("Data not found") : _mapper.Map<GroupDto>(group);
        }

        public async Task AddTeacherAsync(GroupTeacherCreateDto model)
        {
            ArgumentNullException.ThrowIfNull(model);

            if (await _teacherRepository.GetById(model.TeacherId) is null)
            {
                throw new NotFoundException("Teacher not found");
            }

            if (await _groupRepository.GetById(model.GroupId) is null)
            {
                throw new NotFoundException("Group not found");
            }

            var groupTeacher = await _groupTeacherRepository.FindBy(m => m.TeacherId == model.TeacherId && m.GroupId == model.GroupId).FirstOrDefaultAsync();

            if(groupTeacher is not null)  throw new BadRequestException("Teacher is already in this group");

            await _groupTeacherRepository.CreateAsync(_mapper.Map<GroupTeachers>(model));
        }

        public async Task DeleteTeacherAsync(int? id)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));

            var groupTeacher = await _groupTeacherRepository.GetById((int)id) ?? throw new NotFoundException("Data not found");

            await _groupTeacherRepository.DeleteAsync(groupTeacher);
        }
    }
}
