using System;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CarParser.Models;
using CarParser.Parsers;

namespace CarParser
{
    class CarParser
    {
        private static string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\luche\\source\\repos\\CarParser\\CarParser\\Database1.mdf;Integrated Security=True";

        static async Task Main()
        {
            
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            
            var content = await ContentLoader.GetContent("/toyota/?function=getModels&market=EU");
            var ModelsParser = new ModelsParser(content);
            var models = ModelsParser.Parse();
            //Model.AddToDatabase(models, connection);
            Console.WriteLine(models.Count + " items added");

            content = await ContentLoader.GetContent("/toyota/?function=getComplectations&market=EU&model=281220&startDate=198210&endDate=198610");
            var ComplectationsParser = new Parsers.ComplectationParser();

            //var complectations = await ComplectationsParser.ParseFromModels(new List<Model> { models.FirstOrDefault() });
            var complectations = await ComplectationsParser.ParseFromModels(models);

            foreach (var complectation in complectations)
            {
                Console.WriteLine(complectation.ToString());
            }


        }

    }
}