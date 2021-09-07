using System.Collections.Generic;
using Models.ViewModels;

namespace Services.Contracts
{
    public interface IGenericService
    {
        IndexViewModel<T> ApplyPaging<T>(ICollection<T> source, int pageSize, int page = 1);
    }
}