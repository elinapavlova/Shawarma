using System.Collections.Generic;
using System.Linq;
using Models.ViewModels;
using Services.Contracts;

namespace Services
{
    public class GenericService : IGenericService
    {
        public IndexViewModel<T> ApplyPaging<T>(ICollection<T> source, int pageSize, int page = 1)
        {
            var count = source.Count;
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
 
            var pageViewModel = new PageViewModel(count, page, pageSize);
            var viewModel = new IndexViewModel<T>
            {
                PageViewModel = pageViewModel,
                Things =items
            };
            return viewModel;
        }
    }
}