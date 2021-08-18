using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Status;

namespace Services.Contracts
{
    public interface IStatusService
    {
        Task<ResultContainer<ICollection<StatusResponseDto>>> GetStatusList();
        Task<ResultContainer<StatusResponseDto>> GetStatusById(int id);
        Task<ResultContainer<StatusResponseDto>> CreateStatus(StatusRequestDto status);
        Task<ResultContainer<StatusResponseDto>> UpdateStatus(StatusRequestDto status);
        Task<ResultContainer<StatusResponseDto>> DeleteStatus(int id);
    }
}