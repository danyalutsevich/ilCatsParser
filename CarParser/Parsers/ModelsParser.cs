using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarParser.Models;
using AngleSharp.Html.Dom;

namespace CarParser.Parsers
{
    internal class ModelsParser
    {
        private readonly string Content;
        private readonly HtmlParser Parser;
        private readonly IHtmlDocument document;

        public ModelsParser(string content)
        {
            Parser = new HtmlParser();
            Content = content;
            document = Parser.ParseDocument(content);
        }

        public List<Model> Parse()
        {
            var res = new List<Model>();
            var Lists = document.GetElementsByClassName("List");

            foreach (var item in Lists.Skip(1))
            {
                if (item.GetElementsByClassName("Header").Length > 0)
                {
                    var name = item.QuerySelector(".name")?.TextContent;
                    var ids = item.QuerySelectorAll("a");
                    var dataRanges = item.QuerySelectorAll(".dateRange");
                    var modelCodes = item.QuerySelectorAll(".modelCode");

                    for (int i = 0; i < ids.Length; i++)
                    {
                        var id = ids[i].TextContent;
                        var link = ids[i].GetAttribute("href");
                        var dataRange = dataRanges[i].TextContent;
                        var modelCode = modelCodes[i].TextContent;

                        res.Add(new Model(name, modelCode, dataRange, link, id));
                    }
                }
            }
            return res;
        }
    }
}
