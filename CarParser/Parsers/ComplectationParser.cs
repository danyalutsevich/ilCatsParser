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

            foreach (var model in models)
            {
                var complectation = new Complectation();
                var content = await ContentLoader.GetContent(model.Link);
                document = Parser.ParseDocument(content);

                var table = document.GetElementsByTagName("table").FirstOrDefault();
                var rows = table.GetElementsByTagName("tr");

                var th = table.GetElementsByTagName("th");

                foreach (var title in th)
                {
                    //Console.WriteLine(title.TextContent);
                    complectation.Columns.Add(new Column { Title = title.TextContent });
                }

                foreach (var row in rows.Skip(1))
                {
                    var tds = row.GetElementsByTagName("td");
                    if (tds.Length == th.Length)
                    {
                        for (var i = 0; i < tds.Length; i++)
                        {
                            var div = tds[i].GetElementsByTagName("div")?.FirstOrDefault();
                            complectation.Columns[i].Records.Add(div?.TextContent);
                            //Console.Write(div?.GetAttribute("class"));
                            //Console.WriteLine(div?.TextContent);
                        }
                    }
                }
                res.Add(complectation);
            }
            return res;
        }
    }
}
