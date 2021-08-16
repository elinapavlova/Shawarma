using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Status;

namespace Services.Contracts
{
    public interface IStatusService
    {
        Task<ICollection<StatusResponseDto>> GetStatusList();
        Task<StatusResponseDto> GetStatusById(int id);
        void CreateStatus(StatusRequestDto status);
        void UpdateStatus(StatusRequestDto status);
        void DeleteStatus(int id);
    }
}