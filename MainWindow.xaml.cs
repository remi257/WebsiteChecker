using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebsiteChecker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public ObservableCollection<WebsiteWrapper> WebsiteUrls { get; set; } = new ObservableCollection<WebsiteWrapper>();

        public Dictionary<Guid, string> websitesDictionary = new Dictionary<Guid, string>();

        public Visibility MessageBoxVisibility { get => string.IsNullOrWhiteSpace(MessageString) ? Visibility.Hidden : Visibility.Visible; }

        private string messageString;

        public event PropertyChangedEventHandler PropertyChanged;

        private HttpClient httpClient = new HttpClient();

        public string MessageString
        {
            get => messageString;
            set
            {
                if (messageString == value) return;
                messageString = value;
                NotifyPropertyChanged(nameof(MessageBoxVisibility));
                NotifyPropertyChanged();
            }
        }
        private Guid MessageGuid { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            var source = new CancellationTokenSource();
            var ct = source.Token;
            Task.Run(() => 
            {
                while (!ct.IsCancellationRequested)
                {
                    Thread.Sleep(30000);
                    CheckWebsites();
                }
            }, ct);

            Browser.Navigate("https://google.com");
           
        }

        private void CheckWebsites()
        {
            SetMessageString("WebsiteCheck");
        }

        private void AddWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ContentControl control)
            {
                var url = control?.Tag as string;
                if (!url.StartsWith("http")) url = $"http://{url}";
                WebsiteUrls.Add(new WebsiteWrapper(url));
                SetMessageString($"Added website {url}");
            }
            else SetMessageString("cast error");
        }

        protected void NotifyPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void SetMessageString(string message, int miliseconds = 3000)
        {
            var guid = Guid.NewGuid();
            MessageGuid = guid;
            MessageString = message;
            Task.Run(() => {
                Thread.Sleep(miliseconds);
                if (MessageGuid == guid) MessageString = string.Empty;
            });
        }

        private void WebsitesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ListView lv)
            {
                if(lv.SelectedItem is WebsiteWrapper ww)
                {
                    if (!string.IsNullOrWhiteSpace(ww.WebsiteUrlString))
                    {
                        try
                        {
                            Browser.Navigate(ww.WebsiteUrlString);
                        }
                        catch(Exception ex)
                        {
                            SetMessageString(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
