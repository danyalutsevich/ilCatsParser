using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CarParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParser.Parsers
{
    class ComplectationParser
    {
        private readonly HtmlParser Parser;
        private IHtmlDocument document;

        public ComplectationParser()
        {
            Parser = new HtmlParser();
        }

        public async Task<List<Models.Complectation>> ParseFromModels(List<Models.Model> models)
        {
            var res = new List<Models.Complectation>();
            var tasks = new List<Task>();

            foreach (var model in models)
            {
                tasks.Add(Task.Run(async () =>
                {
                    Console.WriteLine("request");
                    var complectation = new Complectation();
                    var content = await ContentLoader.GetContent(model.Link);
                    document = Parser.ParseDocument(content);

                    // First table on a page thats what we looking for
                    var table = document.GetElementsByTagName("table").FirstOrDefault();

                    var rows = table?.GetElementsByTagName("tr");

                    var thS = table?.GetElementsByTagName("th");

                    // Parse header of the table (Titles of the columns)
                    var header = new Row();
                    foreach (var title in thS)
                    {
                        header.Records.Add(title.TextContent);
                    }
                    complectation.Header = header;

                    // Parse rows
                    foreach (var row in rows.Skip(1)) // Skip 1 cause first row is a headers
                    {
                        var record = new Row();

                        var tdS = row.GetElementsByTagName("td");
                        if (tdS.Length == thS.Length)
                        {
                            var a = row.GetElementsByTagName("a").FirstOrDefault();
                            record.Link = a.GetAttribute("href");

                            foreach (var td in tdS)
                            {
                                var div = td.GetElementsByTagName("div").FirstOrDefault();
                                record.Records.Add(div?.TextContent);
                            }
                        }
                        complectation.Rows.Add(record);
                    }
                    complectation.Model = model;
                    res.Add(complectation);
                    Console.WriteLine("parsed");
                }));
                await Task.Delay(500);
            }
            Task.WaitAll(tasks.ToArray());
            return res;
        }
    }
}
