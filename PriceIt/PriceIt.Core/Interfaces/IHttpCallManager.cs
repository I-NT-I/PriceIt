using System.Collections.Generic;
using System.Threading.Tasks;

namespace PriceIt.Core.Interfaces
{
    public interface IHttpCallManager
    {
        int AddHttpCall(string url,string websiteBase);
    }
}
