using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Result;
using Models.Status;

namespace Services.Contracts
{
    public interface IStatusService
    {
        Task<ResultContainer<ICollection<StatusResponseDto>>> GetListByPage(int pageSize, int page = 1);
        Task<ResultContainer<StatusResponseDto>> GetById(int id);
        Task<ResultContainer<StatusResponseDto>> Create(StatusRequestDto status);
        Task<ResultContainer<StatusResponseDto>> Edit(StatusRequestDto status);
        Task<ResultContainer<StatusResponseDto>> Delete(int id);
    }
}