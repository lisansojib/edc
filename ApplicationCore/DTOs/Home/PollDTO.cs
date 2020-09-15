using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class PollDTO : BaseDTO
    {
        public string GraphType { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Panel { get; set; }
        public string Origin { get; set; }
    }
}
