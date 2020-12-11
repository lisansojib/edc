using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class PagedListViewModel<T> where T : class
    {
        public PagedListViewModel(IEnumerable<T> records, int? count)
        {
            Rows = records;
            Total = count ?? 0;
        }

        public int? Total { get; set; }
        public IEnumerable<T> Rows { get; set; }
    }
}
