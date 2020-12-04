using ApplicationCore.DTOs;
using System.Collections.Generic;

namespace Presentation.Admin.Models
{
    public class TeamViewModel : BaseViewModel
    {
        public TeamViewModel()
        {
            ParticipantsList = new List<Select2Option>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Participants { get; set; }
        public List<Select2Option> ParticipantsList { get; set; }
    }
}
