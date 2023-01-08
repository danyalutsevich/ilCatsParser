using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParser.Models
{
    class Group
    {
        public Group()
        {
            Names = new();
            Links = new();
        }

        public List<string> Names { get; set; }
        public List<string> Links { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < Names.Count; i++)
            {
                sb.Append($"{Names[i]} - {Links[i]}");
            }
            return sb.ToString();
        }
    }
}
