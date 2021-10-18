using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
using Infrastructure.Result;
using Models.Error;
using Models.Shawarma;
using Services.Contracts;

namespace Services
{
    public class ShawarmaService : IShawarmaService
    {
        private readonly IShawarmaRepository _repository;
        private readonly IMapper _mapper;
        
        public ShawarmaService
        (
            IShawarmaRepository repository, 
            IMapper mapper
        )
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResultContainer<ICollection<ShawarmaResponseDto>>> GetListByPage
            (int pageSize, bool needOnlyActual, int page = 1)
        {
            var result = _mapper.Map<ResultContainer<ICollection<ShawarmaResponseDto>>>
                (await _repository.GetPage(pageSize, needOnlyActual, page));
            
            return result;
        }

        public async Task<ResultContainer<ICollection<ShawarmaResponseDto>>> GetList()
        {
            var result = _mapper.Map<ResultContainer<ICollection<ShawarmaResponseDto>>>(await _repository.GetList());
            return result;
        }

        public async Task<ResultContainer<ShawarmaResponseDto>> GetById(int id)
        {
            var result = new ResultContainer<ShawarmaResponseDto>();
            var shawarma = await _repository.GetById(id);

            if (shawarma == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(shawarma);

            return result;
        }
        
        public async Task<ResultContainer<ShawarmaResponseDto>> GetByName(string name)
        {
            var result = new ResultContainer<ShawarmaResponseDto>();
            var getShawarma = await _repository.GetShawarmaByName(name);

            if (getShawarma == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.GetShawarmaByName(name));

            return result;
        }

        public async Task<ResultContainer<int>> Count(bool onlyActual)
        {
            var count = new ResultContainer<int>();
            if (!onlyActual)
                count = _mapper.Map<ResultContainer<int>>(await _repository.Count());
            else
                count = _mapper.Map<ResultContainer<int>>(await _repository.CountActual());
            return count;
        }

        public async Task<ResultContainer<ShawarmaResponseDto>> Create(ShawarmaRequestDto shawarmaDto)
        {
            var getShawarma = await GetByName(shawarmaDto.Name);

            if (getShawarma.Data != null)
            {
                getShawarma.ErrorType = ErrorType.BadRequest;
                return getShawarma;
            }

            var shawarma = _mapper.Map<Shawarma>(shawarmaDto);
            var result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.Create(shawarma));
            
            return result;
        }

        public async Task<ResultContainer<ShawarmaResponseDto>> Edit(ShawarmaRequestDto shawarmaDto)
        {
            var getShawarma = await GetByName(shawarmaDto.Name);
            ResultContainer<ShawarmaResponseDto> result;
            
            if (getShawarma.Data == null)
            {
                result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await Create(shawarmaDto));
                return result;
            }
            
            var shawarma = _mapper.Map<Shawarma>(shawarmaDto);
            result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.Edit(shawarma));
            return result;
        }

        public async Task<ResultContainer<ShawarmaResponseDto>> Delete(int id)
        {
            var getShawarma = await GetById(id);

            if (getShawarma.Data == null)
                return getShawarma;

            var result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.Delete(id));
            result.Data = null;
            
            return result;
        }
    }
}