using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

        public ObservableCollection<ICollectionItem> WebsiteUrls { get; set; } = new ObservableCollection<ICollectionItem>();

        public Dictionary<Guid, string> websitesDictionary = new Dictionary<Guid, string>();

        public Visibility MessageBoxVisibility { get => string.IsNullOrWhiteSpace(MessageString) ? Visibility.Hidden : Visibility.Visible; }

        private string messageString;

        private WebsiteCheckerFactory factory = new WebsiteCheckerFactory();

        public event PropertyChangedEventHandler PropertyChanged;

        private string log;
        public string Log
        {
            get => log;
            set
            {
                if (log == value) return;
                log = value;
                NotifyPropertyChanged();
            }
        }


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
            Browser.Navigated += new NavigatedEventHandler(Browser_Navigated);
            Browser.Navigate("https://google.com");
           
        }

        private void CheckWebsites()
        {
            foreach(var ci in WebsiteUrls)
            {
                ci.Process().ForEach(s => SetMessageString(s, true));
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName]string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected void SetMessageString(string message, bool logOnly = false, int miliseconds = 3000)
        {
            Log += $"\n{DateTime.Now.ToShortTimeString()} - {message}";
            if (logOnly) return;
            var guid = Guid.NewGuid();
            MessageGuid = guid;
            MessageString = message;
            Task.Run(() => {
                Thread.Sleep(miliseconds);
                if (MessageGuid == guid) MessageString = string.Empty;
            });
        }

        private static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }

        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }
    }
}
