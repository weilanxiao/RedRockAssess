using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using RedrockAssess.Model;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Composition;
using System.Threading.Tasks;
using System.ComponentModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace RedrockAssess.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListViewPage : Page,INotifyPropertyChanged

    {
        ObservableCollection<Contentlist> list = new ObservableCollection<Contentlist>();
        ObservableCollection<Contentlist> list1 = new ObservableCollection<Contentlist>();
        private string api = @"http://route.showapi.com/255-1?showapi_appid=38525&showapi_sign=b4a7ceb202cf4eba9abd8041b01b7b31&type=41&page=1";
        public bool _isPullRefresh = false;
        public bool IsPullRefresh
        {
            get
            {
                return _isPullRefresh;
            }

            set
            {
                _isPullRefresh = value;
                OnPropertyChanged(nameof(IsPullRefresh));
            }
        }
        public ListViewPage()
        {
            this.InitializeComponent();
            //ListView.ContainerContentChanging += Refresh;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void scrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollRoot.ChangeView(null, 30, null);
        }

        public bool isLoading = false;
        private object o = new object();
        public void Refresh(ListViewBase sender,ContainerContentChangingEventArgs args)
        {
            lock (o)
            {
                if (!isLoading)
                {
                    if (args.ItemIndex == ListView.Items.Count - 1)
                    {
                        isLoading = true;
                        Task.Factory.StartNew(async () =>
                        {
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                            {
                                string content =await NetWork.NetWork.NetWorks(api);
                                string json = GetItem(content);
                                list = JsonConvert.DeserializeObject<ObservableCollection<Contentlist>>(json);
                                ListView.ItemsSource = list;
                            });
                            isLoading = false;
                        });
                    }
                }
            }
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string content = await NetWork.NetWork.NetWorks(api);
            string json = GetItem(content);
            list = JsonConvert.DeserializeObject<ObservableCollection<Contentlist>>(json);
            ListView.ItemsSource = list;
        }

        public string GetItem(string content)
        {
            string _content = "";
            JObject jobject = JObject.Parse(content);
            JToken json0 = jobject["showapi_res_body"];
            JToken json1 = json0["pagebean"];
            JToken json2 = json1["contentlist"];
            _content = json2.ToString();
            return _content;
        }
        public void GetHeight(ListViewItem listItem)
        {
            double height = listItem.ActualHeight;
        }
        public void PausePlay(MediaElement videos)
        {
            videos.Pause();
        }
        public void StartPlay(MediaElement videos)
        {
            videos.AutoPlay = true;
            videos.Play();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //string uri = ((Contentlist)e.ClickedItem).video_uri;
            //MediaElement m = new MediaElement();
            //m.Source = new Uri(uri);
            //StartPlay(m);
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            //GetScroll(ListView, 0);
            //ExperessionAnimation();
            //LoadMoreItemsAsync(2);
        }
        public async void LoadMoreItemsAsync(int count)
        {
            string _count = "page=" + count.ToString();
            string content = await NetWork.NetWork.NetWorks(api.Replace("page=1", _count));
            string json = GetItem(content);
            list1 = JsonConvert.DeserializeObject<ObservableCollection<Contentlist>>(json);
            foreach (var item in list1)
            {
                if (!list1.Equals(item))
                {
                    list.Add(item);
                }
                Debug.WriteLine(list1.Equals(item));
            }
            ListView.ItemsSource = list;
        }

        private ScrollViewer listview_Sc;
        private void GetScroll(DependencyObject o, int n)
        {
            try
            {
                int count = VisualTreeHelper.GetChildrenCount(o);
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var child = VisualTreeHelper.GetChild(o, count - 1);
                        if (n == 0)
                        {
                            if (child is ScrollViewer)
                            {
                                listview_Sc = child as ScrollViewer;
                            }
                            else
                            {
                                GetScroll(VisualTreeHelper.GetChild(o, count - 1), n);
                            }
                        }
                        else if (n == 1)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private Visual rect_1_visual;
        private Visual refreshing_visual;
        private ExpressionAnimation animation2;
        private void ExperessionAnimation()
        {
            refreshing_visual = ElementCompositionPreview.GetElementVisual(listview_Sc);
            var propertyset = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(listview_Sc);
            animation2 = refreshing_visual.Compositor.CreateExpressionAnimation("(PropertySet.Translation.Y==0)?40:0");
            animation2.SetReferenceParameter("PropertySet", propertyset);
            refreshing_visual.StartAnimation("Offset.Y", animation2);
        }

        private int count=1;
        private async void ScrollRoot_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

            if (ScrollRoot.VerticalOffset == ScrollRoot.ScrollableHeight)
            {
                count += 1;
                LoadMoreItemsAsync(count);//滚动条到底部
            }
            var sv = sender as ScrollViewer;

            if (!e.IsIntermediate)
            {
                if (sv.VerticalOffset == 0.0)
                {
                    IsPullRefresh = true;
                    await Task.Delay(2000);
                    string content = await NetWork.NetWork.NetWorks(api);
                    string json = GetItem(content);
                    list = JsonConvert.DeserializeObject<ObservableCollection<Contentlist>>(json);
                    ListView.ItemsSource = list;
                    sv.ChangeView(null, 50, null);
                }
                IsPullRefresh = false;
            }

        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void hateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void downLoadButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ScrollRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
