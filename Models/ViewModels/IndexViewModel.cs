using System.Collections.Generic;

namespace Models.ViewModels
{
    public class IndexViewModel<T>
    {
        public ICollection<T> Things { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}