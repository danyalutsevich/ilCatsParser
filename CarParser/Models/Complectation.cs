using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public static void AddToDatabase(List<Complectation> complectations, SqlConnection connection)
        {

            
            
        }
        
        public static void CreateTable(Model model, Complectation complectation, SqlConnection connection)
        {
            foreach (var column in complectation.Columns)
            {
                using (var command = new SqlCommand($"ALTER TABLE @id ADD {column.Title} nvarchar(255)", connection))
                {
                    command.Parameters.AddWithValue("@id", model.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            foreach (var column in Columns)
            {
                res.Append(column.Title + " ");
            }
            for(var i =0;i < Columns[0].Records.Count; i++)
            {
                res.Append("\n");
                foreach (var column in Columns)
                {
                    res.Append(column.Records[i] + " ");
                }
            }
            
            return res.ToString();
        }
    }
}
