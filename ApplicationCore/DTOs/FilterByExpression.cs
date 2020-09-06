using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTOs
{
    public class FilterByExpression
    {
        public FilterByExpression()
        {
            Parameters = new List<string>();
        }
        public string Expression { get; set; }
        public List<string> Parameters { get; set; }
    }
}
