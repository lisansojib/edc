using System.Collections.Generic;

namespace Presentation.Admin.Models
{
    public class PagedListViewModel
    {
        public PagedListViewModel(IEnumerable<object> records, int? count)
        {
            Rows = records;
            Total = count ?? 0;
        }

        public int? Total { get; set; }
        public IEnumerable<object> Rows { get; set; }
    }
}
