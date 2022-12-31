using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace CarParser
{
    class CarParser
    {
        static async Task Main()
        {
            await Task.Run(() =>
            {
                using (var web = new WebClient())
                {
                    var content = web.DownloadString("https://www.ilcats.ru/toyota/?function=getModels&market=EU");
                    var parser = new HtmlParser();
                    var doc = parser.ParseDocument(content);
                    var Lists = doc.GetElementsByClassName("List");
                    var Headers = doc.GetElementsByClassName("Header");
                    var Names = doc.GetElementsByClassName("name");
                    var Ids = doc.GetElementsByClassName("id");

                    Console.WriteLine(Headers.Length);
                    Console.WriteLine(Ids.Length);

                    var sb = new StringBuilder();

                    foreach (var item in Lists.Skip(1))
                    {
                        if (item.GetElementsByClassName("Header").Length > 0)
                        {
                            var name = item.QuerySelector(".name")?.TextContent;

                            var ids = item.QuerySelectorAll("a");
                            var dataRanges = item.QuerySelectorAll(".dateRange");
                            var modelCode = item.QuerySelectorAll(".modelCode");

                            sb.AppendLine($"Name: {name}");

                            for (int i = 0; i < ids.Length; i++)
                            {
                                var id = ids[i].TextContent;
                                var link = ids[i].GetAttribute("href");
                                var dataRange = dataRanges[i].TextContent;
                                var model = modelCode[i].TextContent;

                                sb.AppendLine($"Id: {id}, DataRange: {dataRange}, Model: {model}, Link: {link}");
                            }
                            sb.AppendLine();
                        }
                    }
                    Console.WriteLine(sb.ToString());
                }
            });
        }
    }
}