using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Shawarma;

namespace Services.Contracts
{
    public interface IShawarmaService
    {
        Task<ResultContainer<ICollection<ShawarmaResponseDto>>> GetShawarmaList();
        Task<ResultContainer<ICollection<ShawarmaResponseDto>>> GetActualShawarmaList();
        Task<ResultContainer<ShawarmaResponseDto>> GetShawarmaById(int id);
        Task<ResultContainer<ShawarmaResponseDto>> GetShawarmaByName(string name);
        Task<ResultContainer<ShawarmaResponseDto>> CreateShawarma(ShawarmaRequestDto shawarma);
        Task<ResultContainer<ShawarmaResponseDto>> UpdateShawarma(ShawarmaRequestDto shawarma);
        Task<ResultContainer<ShawarmaResponseDto>> DeleteShawarma(int id);
    }
}