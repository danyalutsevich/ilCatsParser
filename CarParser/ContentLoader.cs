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
        string BaseUrl { get; set; } = "https://www.ilcats.ru";
        string Uri { get; set; }

        public ContentLoader(string uri)
        {
            Uri = uri;
        }

        public async Task<string> GetContent()
        {
            using (var web = new WebClient())
            {
                var url = $"{BaseUrl}/{Uri}";
                var content = await web.DownloadStringTaskAsync(url);
                return content;
            }
        }
    }
}
