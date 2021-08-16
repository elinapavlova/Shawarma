using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Models.Status;
using Services.Contracts;

namespace Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repository;
        private readonly IMapper _mapper;
        
        public StatusService(IMapper mapper, IStatusRepository repository)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ICollection<StatusResponseDto>> GetStatusList()
        {
            var statuses = _mapper.Map<ICollection<StatusResponseDto>>(await _repository.GetStatusList());
            return statuses;
        }

        public async Task<StatusResponseDto> GetStatusById(int id)
        {
            var status = _mapper.Map<StatusResponseDto>(await _repository.GetStatusById(id));
            return status;
        }

        public void CreateStatus(StatusRequestDto statusDto)
        {
            var status = _mapper.Map<Status>(statusDto);
            _repository.CreateStatus(status);
        }

        public void UpdateStatus(StatusRequestDto statusDto)
        {
            var status = _mapper.Map<Status>(statusDto);
            _repository.UpdateStatus(status);
        }

        public void DeleteStatus(int id)
        {
            _repository.DeleteStatus(id);
        }
    }
}