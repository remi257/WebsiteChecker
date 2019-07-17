using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteChecker
{
    class WebsiteCheckerFactory
    {
        public IUrlDataGetter<string[]> CreateCssClassFirstChildPropertyArrayGetter(string cssClassName, string propertyName)
        {
            return new UrlDataGetter<string[]>((url) => 
            {
                var web = new HtmlAgilityPack.HtmlWeb();
                var doc = web.Load(url);
                return doc.DocumentNode.SelectNodes($"//*[contains(@class,'{cssClassName}')]").Select(node => node.FirstChild.GetAttributeValue(propertyName, string.Empty)).ToArray();
            });
        }

        public IUrlDataComparator<string[]> GetDefaultArrayOfStringComparator()
        {
            return new ArrayOfStringComparator();
        }


        
    }

}
