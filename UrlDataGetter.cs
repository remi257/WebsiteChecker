using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteChecker
{
    class UrlDataGetter<T> : IUrlDataGetter<T>
    {
        private Func<string, T> convertFunc;

        public UrlDataGetter(Func<string, T> convertFunc)
        {
            this.convertFunc = convertFunc;
        }

        public T GetData(string url)
        {
            return convertFunc(url);
        }
    }
}
