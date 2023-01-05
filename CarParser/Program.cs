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
            
            var contentLoader = new ContentLoader("/toyota/?function=getModels&market=EU");
            var content = await contentLoader.GetContent();
            var ModelsParser = new ModelsParser(content);
            var models = ModelsParser.Parse();

            Model.AddToDatabase(models, connection);
            
            
            Console.WriteLine(models.Count+ " items added");

            foreach (var model in models)
            {
                //Console.WriteLine(model);
            }

        }

    }
}