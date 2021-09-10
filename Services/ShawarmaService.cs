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
        
        public ShawarmaService(IShawarmaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResultContainer<ICollection<ShawarmaResponseDto>>> GetShawarmaListByPage
            (int pageSize, bool needOnlyActual, int page = 1)
        {
            var result = _mapper.Map<ResultContainer<ICollection<ShawarmaResponseDto>>>
                (await _repository.GetPage(pageSize, needOnlyActual, page));
            
            return result;
        }

        public async Task<ResultContainer<ShawarmaResponseDto>> GetShawarmaById(int id)
        {
            var result = new ResultContainer<ShawarmaResponseDto>();
            
            var getShawarma = await _repository.GetById(id);

            if (getShawarma == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.GetById(id));

            return result;
        }
        
        public async Task<ResultContainer<ShawarmaResponseDto>> GetShawarmaByName(string name)
        {
            ResultContainer<ShawarmaResponseDto> result = new ResultContainer<ShawarmaResponseDto>();
            
            var getShawarma = await _repository.GetShawarmaByName(name);

            if (getShawarma == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }
            
            result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.GetShawarmaByName(name));

            return result;
        }

        public async Task<int> Count()
        {
            var count = await _repository.Count();
            return count;
        }

        public async Task<ResultContainer<ShawarmaResponseDto>> CreateShawarma(ShawarmaRequestDto shawarmaDto)
        {
            var getShawarma = await GetShawarmaByName(shawarmaDto.Name);

            if (getShawarma.Data != null)
            {
                getShawarma.ErrorType = ErrorType.BadRequest;
                return getShawarma;
            }

            var shawarma = _mapper.Map<Shawarma>(shawarmaDto);
            var result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.Create(shawarma));
            
            return result;
        }

        public async Task<ResultContainer<ShawarmaResponseDto>> UpdateShawarma(ShawarmaRequestDto shawarmaDto)
        {
            var getShawarma = await GetShawarmaByName(shawarmaDto.Name);
            ResultContainer<ShawarmaResponseDto> result;
            
            if (getShawarma.Data == null)
            {
                result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await CreateShawarma(shawarmaDto));
                return result;
            }
            
            var shawarma = _mapper.Map<Shawarma>(shawarmaDto);
            result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.Edit(shawarma));
            return result;
        }

        public async Task<ResultContainer<ShawarmaResponseDto>> DeleteShawarma(int id)
        {
            var getShawarma = await GetShawarmaById(id);

            if (getShawarma.Data == null)
                return getShawarma;

            var result = _mapper.Map<ResultContainer<ShawarmaResponseDto>>(await _repository.Delete(id));
            result.Data = null;
            
            return result;
        }
    }
}