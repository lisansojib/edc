using Microsoft.AspNetCore.Http;
using System;

namespace Presentation.Admin.Models
{
    public class PollBindingModel : BaseViewModel
    {
        public string GraphType { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Panel { get; set; }
        public string Origin { get; set; }
    }
}
