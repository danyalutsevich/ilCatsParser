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
            #region Models
            Console.WriteLine("Models");
            var content = await ContentLoader.GetContent("/toyota/?function=getModels&market=EU");
            var ModelsParser = new ModelsParser(content);
            var models = ModelsParser.Parse();
            Model.AddToDatabase(models, connection);
            Console.WriteLine(models.Count + " items added");
            #endregion
            #region Complectations
            Console.WriteLine("Complectations");
            content = await ContentLoader.GetContent(models.FirstOrDefault().Link);
            var ComplectationsParser = new ComplectationParser();
            var complectations = await ComplectationsParser.ParseFromModels(models);
            //var complectations = await ComplectationsParser.ParseFromModels(new List<Model> { models.FirstOrDefault() });


            foreach (var complectation in complectations)
            {
                Console.WriteLine(complectation.ToString());
                complectation.AddToDatabase(connection);
            }
            #endregion
            //#region Groups  
            //Console.WriteLine("Groups");
            //var groupParser = new GroupParser();
            //var groups = await groupParser.ParseFromComplectation(complectations.FirstOrDefault());
            //foreach (var group in groups)
            //{
            //    Console.WriteLine(group);
            //}
            //#endregion


        }
    }
}