using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Rooms;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepo;
        private readonly IMapper _mapper;
        public RoomService(IRoomRepository roomRepo,
                          IMapper mapper)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
        }
        public async Task CreateAsync(RoomCreateDto model)
        {
            await _roomRepo.CreateAsync(_mapper.Map<Room>(model));
        }

        public async Task EditAsync(int? id, RoomEditDto model)
        {
            if (id is null) throw new ArgumentNullException();

            var room = await _roomRepo.GetById((int)id);

            if (room is null) throw new NotFoundException("Data was not found");

            await _roomRepo.EditAsync(_mapper.Map(model, room));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var room = await _roomRepo.GetById((int)id);

            if (room is null) throw new NotFoundException("Data was not found");

            await _roomRepo.DeleteAsync(room);
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<RoomDto>>(await _roomRepo.GetAllAsync());
        }

        public async Task<RoomDto> GetByIdAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var room = await _roomRepo.GetById((int)id);

            if (room is null) throw new NotFoundException("Data was not found");

            return _mapper.Map<RoomDto>(room);
        }
    }
}
