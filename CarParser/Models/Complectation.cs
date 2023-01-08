using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
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
            Links = new List<string>();
            Groups = new List<Group>();
        }

        public List<Column>? Columns { get; set; }
        public Model Model { get; set; }
        public List<string> Links { get; set; }
        public List<Group> Groups { get; set; }

        public void AddToDatabase(SqlConnection connection)
        {
            CreateTable(connection);
            try
            {
                for (var i = 0; i < Columns[0].Records.Count; i++)
                {
                    var command = new StringBuilder($"INSERT INTO [{Model.Id}] VALUES (");
                    foreach (var column in Columns)
                    {
                        command.Append($"'{column.Records[i]}',");
                    }

                    command.Remove(command.Length - 1, 1); // remove last comma
                    command.Append(")");
                    var sqlCommand = new SqlCommand(command.ToString(), connection);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateTable(SqlConnection connection)
        {
            try
            {
                var command = new StringBuilder($"CREATE TABLE [{Model.Id}] (");

                var columns = GetColumns("[", "] NVARCHAR(100),");
                command.Append(columns);
                command.Append(")");

                var sqlCommand = new SqlCommand(command.ToString(), connection);
                sqlCommand.ExecuteNonQuery();
                CreateUniqueIndex(connection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateUniqueIndex(SqlConnection connection)
        {
            try
            {
                var command = new StringBuilder($"CREATE UNIQUE INDEX table_index ON [{Model.Id}]({GetColumns("[", "],")}) ");
                command = command.Remove(command.Length - 3, 1); // remove last comma
                var sqlCommand = new SqlCommand(command.ToString(), connection);
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string GetColumns(string prefix, string postfix)
        {
            var result = new StringBuilder();
            foreach (var column in Columns)
            {
                result.Append($"{prefix}{column.Title}{postfix}");
            }
            return result.ToString();
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
                res.AppendLine($"Link: {Links[i]}");
                foreach (var column in Columns)
                {
                    res.Append(column.Records[i] + " ");
                }
            }
            return res.ToString();
        }
    }
}
