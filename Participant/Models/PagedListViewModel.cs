using System.Collections.Generic;

namespace Presentation.Participant.Models
{
    public class PagedListViewModel
    {
        public PagedListViewModel(IEnumerable<object> records, int? count)
        {
            Rows = records;
            Total = count;
        }

        public int? Total { get; set; }
        public IEnumerable<object> Rows { get; set; }
    }
}
