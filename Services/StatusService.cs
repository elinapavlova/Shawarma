using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models.Error;
using Models.Status;
using Services.Contracts;

namespace Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _repository;
        private readonly IMapper _mapper;
        
        public StatusService
        (
            IMapper mapper, 
            IStatusRepository repository
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResultContainer<ICollection<StatusResponseDto>>> GetListByPage(int pageSize, int page = 1)
        {
            var result = _mapper.Map<ResultContainer<ICollection<StatusResponseDto>>>
                (await _repository.GetPage(pageSize, page));
            
            return result;
        }

        public async Task<ResultContainer<StatusResponseDto>> GetById(int id)
        {
            var result = new ResultContainer<StatusResponseDto>();
            var getStatus = await _repository.GetById(id);
            
            if (getStatus == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<StatusResponseDto>>(await _repository.GetById(id));

            return result;
        }

        public async Task<ResultContainer<StatusResponseDto>> Create(StatusRequestDto statusDto)
        {
            var getStatus = await GetById(statusDto.Id);
            
            if (getStatus.Data != null)
            {
                getStatus.ErrorType = ErrorType.BadRequest;
                return getStatus;
            }
            
            var status = _mapper.Map<Status>(statusDto);
            var result = _mapper.Map<ResultContainer<StatusResponseDto>>(await _repository.Create(status));

            return result;
        }

        public async Task<ResultContainer<StatusResponseDto>> Edit(StatusRequestDto statusDto)
        {
            var getStatus = await GetById(statusDto.Id);
            ResultContainer<StatusResponseDto> result;
            
            if (getStatus.Data == null)
            {
                result = _mapper.Map<ResultContainer<StatusResponseDto>>(await Create(statusDto));
                return result;
            }

            var status = _mapper.Map<Status>(statusDto);    
            result = _mapper.Map<ResultContainer<StatusResponseDto>>(await _repository.Edit(status));

            return result;
        }

        public async Task<ResultContainer<StatusResponseDto>> Delete(int id)
        {
            var getStatus = await GetById(id);
            
            if (getStatus.Data == null)
                return getStatus;
            
            var result = _mapper.Map<ResultContainer<StatusResponseDto>>(await _repository.Delete(id));
            result.Data = null;

            return result;
        }
    }
}