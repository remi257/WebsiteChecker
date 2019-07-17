using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteChecker
{
    public class WebsiteWrapper : INotifyPropertyChanged
    {
        public WebsiteWrapper(string websiteUrlString)
        {
            WebsiteUrlString = websiteUrlString;
            Id = Guid.NewGuid();
        }

        private string websiteUrlString;
        public string WebsiteUrlString
        {
            get => websiteUrlString;
            set
            {
                if (websiteUrlString == value) return;
                websiteUrlString = value;
                NotifyPropertyChanged();
            }
        }

        private Guid id;

        public Guid Id
        {
            get => id;
            set
            {
                if (id == value) return;
                id = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => WebsiteUrlString;
        

        protected void NotifyPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
