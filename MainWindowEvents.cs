using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WebsiteChecker
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private void AddWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ContentControl control)
            {
                var url = control?.Tag as string;
                if (!url.StartsWith("http")) url = $"http://{url}";
                WebsiteUrls.Add(new CollectionItem<string[]>(url, factory.CreateCssClassFirstChildPropertyArrayGetter("entry-title", "title"), factory.GetDefaultArrayOfStringComparator()));
                SetMessageString($"Added website {url}");
            }
            else SetMessageString("cast error");
        }

        private void Browser_Navigated(object sender, NavigationEventArgs e)
        {
            SetSilent(Browser, true); // make it silent
        }

        private void WebsitesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView lv)
            {
                if (lv.SelectedItem is ICollectionItem ci)
                {
                    ci.Check();
                    if (!string.IsNullOrWhiteSpace(ci.Url))
                    {
                        try
                        {
                            Browser.Navigate(ci.Url);
                        }
                        catch (Exception ex)
                        {
                            SetMessageString(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
