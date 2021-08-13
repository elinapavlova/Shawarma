using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Contracts;
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
        
        public async Task<ICollection<ShawarmaResponseDto>> GetShawarmaList()
        {
            var shawarmas = _mapper.Map<ICollection<ShawarmaResponseDto>>(await _repository.GetShawarmaList());
            return shawarmas;
        }

        public async Task<ShawarmaResponseDto> GetShawarmaById(int id)
        {
            var shawarma = _mapper.Map<ShawarmaResponseDto>(await _repository.GetShawarmaById(id));
            return shawarma;
        }

        public void CreateShawarma(ShawarmaRequestDto shawarmaDto)
        {
            if (shawarmaDto.Name == null) return;
            
            var shawarma = _mapper.Map<Shawarma>(shawarmaDto);
            _repository.CreateShawarma(shawarma);
        }

        public void UpdateShawarma(int id, ShawarmaRequestDto shawarmaDto)
        {
            if (shawarmaDto.Name == null) return;
            
            var user = _mapper.Map<Shawarma>(shawarmaDto);
            _repository.UpdateShawarma(id, user);
        }

        public void DeleteShawarma(int id)
        {
            _repository.DeleteShawarma(id);
        }
    }
}