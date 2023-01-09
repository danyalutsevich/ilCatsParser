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
            var AllGroups = new List<Group>();
            var tasks = new List<Task>();

            foreach (var row in complectation.Rows)
            {
                Console.WriteLine("request");
                tasks.Add(Task.Run(async () =>
                {
                    var content = await ContentLoader.GetContent(row.Link);
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
                    row.Group = group;
                    AllGroups.Add(group);
                    Console.WriteLine("parsed");
                }));
                await Task.Delay(1000);
            }
            Task.WaitAll(tasks.ToArray());
            return AllGroups;
        }
    }
}
