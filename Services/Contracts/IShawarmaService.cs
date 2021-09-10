using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Shawarma;

namespace Services.Contracts
{
    public interface IShawarmaService
    {
        Task<ResultContainer<ICollection<ShawarmaResponseDto>>> GetShawarmaListByPage
            (int pageSize, bool needOnlyActual, int page = 1);
        Task<ResultContainer<ShawarmaResponseDto>> GetShawarmaById(int id);
        Task<ResultContainer<ShawarmaResponseDto>> GetShawarmaByName(string name);
        Task<int> Count();
        Task<ResultContainer<ShawarmaResponseDto>> CreateShawarma(ShawarmaRequestDto shawarma);
        Task<ResultContainer<ShawarmaResponseDto>> UpdateShawarma(ShawarmaRequestDto shawarma);
        Task<ResultContainer<ShawarmaResponseDto>> DeleteShawarma(int id);
    }
}