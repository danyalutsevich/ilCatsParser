using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CarParser.Models;

namespace CarParser.Parsers
{
    class GroupParser
    {
        public async Task<List<Group>> ParseFromComplectation(Complectation complectation)
        {
            var res = new List<Group>();
            var tasks = new List<Task>();

            foreach (var link in complectation.Links)
            {
                Console.WriteLine("request");
                tasks.Add(Task.Run(async () =>
                {
                    var content = await ContentLoader.GetContent(link);
                    var parser = new HtmlParser();
                    var document = parser.ParseDocument(content);
                    var divS = document.GetElementsByClassName("name");
                    var group = new Group();

                    foreach (var div in divS)
                    {
                        var a = div.GetElementsByTagName("a").FirstOrDefault();
                        group.Names.Add(a.TextContent);
                        group.Links.Add(a.GetAttribute("href"));
                    }
                    res.Add(group);
                    Console.WriteLine("parsed");
                }));
                await Task.Delay(1000);
            }
            Task.WaitAll(tasks.ToArray());
            complectation.Groups.AddRange(res);
            return res;
        }
    }
}
