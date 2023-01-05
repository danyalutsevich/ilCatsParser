using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParser.Models
{
    class Column
    {
        public Column()
        {
            Records = new List<string>();
        }
        public string? Title { get; set; }
        public List<string>? Records { get; set; }  
    }

    class Complectation
    {
        public Complectation()
        {
            Columns = new List<Column>();
        }
        
        public List<Column>? Columns { get; set; }

        

    }
}
