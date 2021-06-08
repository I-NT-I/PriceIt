using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PriceIt.Core.Interfaces
{
    public interface IHttpCallManager
    {
        int AddHttpCall(string url,string websiteBase);

        Task<List<HtmlDocument>> CallStack();

        Task<HtmlDocument> CallWebsite(string url,string websiteBase);
    }
}
