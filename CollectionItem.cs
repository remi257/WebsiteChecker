using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteChecker
{
    class CollectionItem<T> : ICollectionItem, INotifyPropertyChanged
    {
        readonly string url;
        IUrlDataGetter<T> getter;
        IUrlDataComparator<T> comparator;
        T lastData;
        bool changedSinceLastCheck;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ChangedSinceLastCheck
        {
            get => changedSinceLastCheck;
            set
            {
                if (changedSinceLastCheck == value) return;
                changedSinceLastCheck = value;
                NotifyPropertyChanged();
            }
        }

        public CollectionItem(string url, IUrlDataGetter<T> getter, IUrlDataComparator<T> comparator)
        {
            this.url = url;
            this.getter = getter;
            this.comparator = comparator;
            this.changedSinceLastCheck = false;
            this.lastData = default(T);
        }

        public string Url => url;



        public List<string> Process()
        {
            var newData = getter.GetData(url);
            var messages =  comparator.CompareData(lastData, newData, out bool change);
            changedSinceLastCheck |= change;
            if (change) lastData = newData;
            return messages;
        }

        protected void NotifyPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override string ToString() => url;

        public void Check()
        {
            this.ChangedSinceLastCheck = false;
        }
    }

    public interface ICollectionItem
    {
        string Url { get; }
        List<string> Process();
        void Check();
        bool ChangedSinceLastCheck { get; set; }
    }
}
