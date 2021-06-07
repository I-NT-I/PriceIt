using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
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

        public async Task<List<HtmlDocument>> CallStack()
        {
            HtmlDocument doc = null;
            var results = new List<HtmlDocument>();

            for (var i = 0; i < _urls.Count; i++)
            {
                var website = new HtmlWeb { AutoDetectEncoding = false, OverrideEncoding = Encoding.Default, UseCookies = true, UserAgent = _urls[i].Agent };

                //wait if the same website is called more than once in a row 
                //must test or scramble the calls first 
                if (i != 0 && _urls[i - 1].WebsiteBase == _urls[i].WebsiteBase)
                {
                    //System.Threading.Thread.Sleep(RandomNumberGenerator.GetInt32(1000, 1999));
                    System.Threading.Thread.Sleep(1000);
                }

                try
                {
                    doc = await website.LoadFromWebAsync(_urls[i].WebsiteBase + _urls[i].Url);
                }
                catch (Exception e)
                {
                    // ignored
                    //should log
                    Debug.WriteLine("page exception");
                }

                results.Add(doc);
                Debug.WriteLine("page added");
            }

            return results;
        }

        public async Task<HtmlDocument> CallWebsite(string url, string websiteBase)
        {
            HtmlDocument doc = null;

            var website = new HtmlWeb { AutoDetectEncoding = false, OverrideEncoding = Encoding.Default, UseCookies = true, UserAgent = UserAgentBase + AgentGenerator.GetRandomCode(UserAgentCodeLength) };

            try
            {
                doc = await website.LoadFromWebAsync(websiteBase + url);
            }
            catch (Exception e)
            {
                // ignored
                //should log
            }

            return doc;
        }
    }
}
