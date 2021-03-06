using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Shawarma;

namespace Services.Contracts
{
    public interface IShawarmaService
    {
        Task<ResultContainer<ICollection<ShawarmaResponseDto>>> GetListByPage
            (int pageSize, bool needOnlyActual, int page = 1);
        Task<ResultContainer<ICollection<ShawarmaResponseDto>>> GetList();
        Task<ResultContainer<ShawarmaResponseDto>> GetById(int id);
        Task<ResultContainer<ShawarmaResponseDto>> GetByName(string name);
        Task<ResultContainer<int>> Count(bool onlyActual);
        Task<ResultContainer<ShawarmaResponseDto>> Create(ShawarmaRequestDto shawarma);
        Task<ResultContainer<ShawarmaResponseDto>> Edit(ShawarmaRequestDto shawarma);
        Task<ResultContainer<ShawarmaResponseDto>> Delete(int id);
    }
}