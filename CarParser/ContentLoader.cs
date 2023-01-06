using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarParser
{
    class ContentLoader
    {
        private static string BaseUrl { get; set; } = "https://www.ilcats.ru";

        public static async Task<string> GetContent(string uri)
        {
            using (var web = new WebClient())
            {
                var url = $"{BaseUrl}/{uri}";
                var content = await web.DownloadStringTaskAsync(url);
                return content;
            }
        }
    }
}
