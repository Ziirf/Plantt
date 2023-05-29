using AutoMapper;
using Plantt.Domain.DTOs.Room.Request;
using Plantt.Domain.Entities;
using Plantt.Domain.Interfaces;
using Plantt.Domain.Interfaces.Services.EntityServices;

namespace Plantt.Applcation.Services.EntityServices
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RoomEntity?> GetRoomByIdAsync(int roomId)
        {
            return await _unitOfWork.RoomRepository.GetByIdAsync(roomId);
        }

        public IEnumerable<RoomEntity> GetAccountsRooms(int accountId)
        {
            return _unitOfWork.RoomRepository.GetAllRooms(accountId);
        }

        public async Task<RoomEntity> CreateRoomAsync(UpdateRoomRequest roomRequest)
        {
            var room = _mapper.Map<RoomEntity>(roomRequest);

            await _unitOfWork.RoomRepository.AddAsync(room);
            await _unitOfWork.CommitAsync();

            return room;
        }

        public async Task<RoomEntity> UpdateRoomAsync(UpdateRoomRequest roomRequest, RoomEntity room)
        {
            room.HomeId = roomRequest.HomeId;
            room.IsOutside = roomRequest.IsOutside;
            room.Name = roomRequest.Name;
            room.SunlightLevel = roomRequest.SunlightLevel;

            _unitOfWork.RoomRepository.Update(room);
            await _unitOfWork.CommitAsync();

            return room;
        }

        public async Task DeleteRoomAsync(RoomEntity roomEntity)
        {
            _unitOfWork.RoomRepository.Delete(roomEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<bool> ValidateOwnerAsync(int roomId, int accountId)
        {
            return await _unitOfWork.RoomRepository.IsValidOwnerAsync(roomId, accountId);
        }
    }
}
