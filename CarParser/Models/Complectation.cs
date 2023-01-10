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

    class Row
    {
        public Row()
        {
            Records = new List<string>();
            Link = string.Empty;
        }

        public List<string> Records { get; set; }
        public string Link { get; set; }
        public Group Group { get; set; }
        public Complectation Complectation { get; set; }

        public string GetLink()
        {
            return $"/toyota/?function=getGroups&market=EU&model={Complectation.Model.Id}&modification={Records[0]}";
        }

        public string ToString(string prefix = "", string postfix = "", bool removeLastChar = false)
        {
            var sb = new StringBuilder();
            foreach (var record in Records)
            {
                sb.Append($"{prefix}{record}{postfix}");
            }
            if (removeLastChar)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }

    class Complectation
    {
        public Complectation()
        {
            Rows = new List<Row>();
            Header = new Row();
        }

        public List<Row> Rows { get; set; }
        public Row Header { get; set; }

        public Model Model { get; set; }

        public void AddToDatabase(SqlConnection connection)
        {
            CreateTable(connection);
            try
            {
                foreach (var row in Rows)
                {
                    var command = new StringBuilder($"INSERT INTO [{Model.Id}] VALUES (");
                    command.Append(row.ToString("'", "',", true));
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
                command.Append(Header.ToString("[", "] NVARCHAR(100),", true));
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
                var command = new StringBuilder($"CREATE UNIQUE INDEX table_index ON [{Model.Id}]({Header.ToString("[", "],", true)})");
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
            res.AppendLine(Header.ToString(postfix: " | "));

            foreach (var row in Rows)
            {
                res.AppendLine(row.ToString(postfix: " | "));
            }

            return res.ToString();
        }
    }
}
