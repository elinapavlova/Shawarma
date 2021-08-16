using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Shawarma;

namespace Services.Contracts
{
    public interface IShawarmaService
    {
        Task<ICollection<ShawarmaResponseDto>> GetShawarmaList();
        Task<ShawarmaResponseDto> GetShawarmaById(int id);
        void CreateShawarma(ShawarmaRequestDto shawarma);
        void UpdateShawarma(ShawarmaRequestDto shawarma);
        void DeleteShawarma(int id);
    }
}