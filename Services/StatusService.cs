using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Error;
using Infrastructure.Result;
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
        public async Task<ResultContainer<ICollection<StatusResponseDto>>> GetStatusList()
        {
            var statuses = _mapper.Map<ResultContainer<ICollection<StatusResponseDto>>>
                (await _repository.GetStatusList());
            return statuses;
        }

        public async Task<ResultContainer<StatusResponseDto>> GetStatusById(int id)
        {
            ResultContainer<StatusResponseDto> result = new ResultContainer<StatusResponseDto>();
            
            var getStatus = await _repository.GetStatusById(id);
            
            if (getStatus == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<StatusResponseDto>>(await _repository.GetStatusById(id));

            return result;
        }

        public async Task<ResultContainer<StatusResponseDto>> CreateStatus(StatusRequestDto statusDto)
        {
            var getStatus = await GetStatusById(statusDto.Id);
            
            if (getStatus.Data != null)
            {
                getStatus.ErrorType = ErrorType.BadRequest;
                return getStatus;
            }
            
            var status = _mapper.Map<Status>(statusDto);
            var result = _mapper.Map<ResultContainer<StatusResponseDto>>(await _repository.CreateStatus(status));

            return result;
        }

        public async Task<ResultContainer<StatusResponseDto>> UpdateStatus(StatusRequestDto statusDto)
        {
            var getStatus = await GetStatusById(statusDto.Id);
            
            if (getStatus.Data == null)
                return getStatus;

            var status = _mapper.Map<Status>(statusDto);
            var result = _mapper.Map<ResultContainer<StatusResponseDto>>(await _repository.UpdateStatus(status));

            return result;
        }

        public async Task<ResultContainer<StatusResponseDto>> DeleteStatus(int id)
        {
            var getStatus = await GetStatusById(id);
            
            if (getStatus.Data == null)
                return getStatus;
            
            var result = _mapper.Map<ResultContainer<StatusResponseDto>>(await _repository.DeleteStatus(id));
            result.Data = null;

            return result;
        }
    }
}