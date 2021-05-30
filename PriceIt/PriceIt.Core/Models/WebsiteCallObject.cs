using System;
using System.Collections.Generic;
using System.Text;

namespace PriceIt.Core.Models
{
    public class WebsiteCallObject
    {
        public string Url;
        public string WebsiteBase;
        public string Agent;

        public WebsiteCallObject(string url,string websiteBase, string agent)
        {
            Url = url;
            WebsiteBase = websiteBase;
            Agent = agent;
        }
    }
}
