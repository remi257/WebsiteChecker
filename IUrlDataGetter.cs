using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteChecker
{
    public interface IUrlDataGetter<T>
    {
        T GetData(string url);
    }
}
