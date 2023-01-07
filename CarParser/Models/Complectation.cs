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
        public Model model { get; set; }

        public static void AddToDatabase(Model model, List<Complectation> complectations, SqlConnection connection)
        {
            foreach (var complectation in complectations)
            {
                CreateTable(model, complectation, connection);
                for (var i = 0; i < complectation.Columns[0].Records.Count; i++)
                {
                    var command = new StringBuilder($"INSERT INTO [{model.Id}] VALUES (");
                    foreach (var column in complectation.Columns)
                    {
                        command.Append($"'{column.Records[i]}',");
                    }
                    command.Remove(command.Length - 1, 1);
                    command.Append(")");
                    var sqlCommand = new SqlCommand(command.ToString(), connection);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        private static void CreateTable(Model model, Complectation complectation, SqlConnection connection)
        {
            try
            {

                var command = new StringBuilder($"CREATE TABLE [{model.Id}] (");
                foreach (var column in complectation.Columns)
                {
                    command.Append($"[{column.Title}] NVARCHAR(100),");
                }
                command.Append(")");

                var sqlCommand = new SqlCommand(command.ToString(), connection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override string ToString()
        {
            var res = new StringBuilder();
            foreach (var column in Columns)
            {
                res.Append(column.Title + " ");
            }
            for (var i = 0; i < Columns[0].Records.Count; i++)
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
