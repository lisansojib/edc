using System.Collections.Generic;

namespace Presentation.Admin.Models
{
    public class TeamBindingModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<int> Participants { get; set; }
    }
}
