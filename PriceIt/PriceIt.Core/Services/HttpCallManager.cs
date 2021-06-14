using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PriceIt.Core.Helpers;
using PriceIt.Core.Interfaces;
using PriceIt.Core.Models;

namespace PriceIt.Core.Services
{
    public class HttpCallManager : IHttpCallManager
    {
        private readonly List<WebsiteCallObject> _urls;
        private const string UserAgentBase = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36";
        private const int UserAgentCodeLength = 5;

        public HttpCallManager()
        {
            _urls = new List<WebsiteCallObject>();
        }

        public int AddHttpCall(string url,string websiteBase)
        {
            //var website = new WebsiteCallObject(url, websiteBase, UserAgentBase + AgentGenerator.GetRandomCode(UserAgentCodeLength));
            var website = new WebsiteCallObject(url, websiteBase, UserAgentBase);
            _urls.Add(website);

            return _urls.IndexOf(website);
        }
    }
}
