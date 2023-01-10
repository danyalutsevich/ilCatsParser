using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParser.Models
{
    class Model
    {
        public string Name { get; set; }
        public string ModelCode { get; set; }
        public string DateRange { get; set; }
        public string Id { get; set; }
        public string Link { get; set; }

        public Model(string name, string modelCode, string dateRange, string link, string id)
        {
            Name = name;
            ModelCode = modelCode;
            DateRange = dateRange;
            Link = link;
            Id = id;
        }

        public override string ToString()
        {
            return $"Name: {Name}, ModelCode: {ModelCode}, DateRange: {DateRange}, Id: {Id}, Link: {Link}";
        }

        public static void AddToDatabase(List<Model> models, SqlConnection connection)
        {
            foreach (var model in models)
            {
                try
                {
                    var command = new SqlCommand("INSERT INTO Models (id, name, model_code, date_range) VALUES (@id,@name,@model_code,@date_range)", connection);
                    command.Parameters.AddWithValue("@id", model.Id);
                    command.Parameters.AddWithValue("@model_code", model.ModelCode);
                    command.Parameters.AddWithValue("@date_range", model.DateRange);
                    command.Parameters.AddWithValue("@name", model.Name);

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
