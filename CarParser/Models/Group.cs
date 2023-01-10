using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
        public string ComplectationModelCode { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < Names.Count; i++)
            {
                sb.Append($"{Names[i]} - {Links[i]}");
            }
            return sb.ToString();
        }

        public void AddToDatabase(SqlConnection connection)
        {
            CreateTable(connection);
            for (var i = 0; i < Names.Count; i++)
            {
                try
                {
                    var command = new SqlCommand("INSERT INTO @table_name (name,link) VALUES (@name,@link)", connection);
                    command.Parameters.AddWithValue("@name", Names[i]);
                    command.Parameters.AddWithValue("@link", Links[i]);
                    command.Parameters.AddWithValue("@table_name", ComplectationModelCode);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void CreateTable(SqlConnection connection)
        {
            try
            {
                var command = new SqlCommand("CREATE TABLE @table_name (name NVARCHAR(200), link NVARCHAR(200))", connection);
                command.Parameters.AddWithValue("@table_name", ComplectationModelCode);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
