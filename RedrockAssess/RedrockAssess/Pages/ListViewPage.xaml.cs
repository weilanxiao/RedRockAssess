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
using Windows.Storage;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace RedrockAssess.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListViewPage : Page, INotifyPropertyChanged
    {
        ObservableCollection<Contentlist> list = new ObservableCollection<Contentlist>();
        ObservableCollection<Contentlist> list1 = new ObservableCollection<Contentlist>();
        private string api = @"http://route.showapi.com/255-1?showapi_appid=38525&showapi_sign=b4a7ceb202cf4eba9abd8041b01b7b31&type=41&page=1";
        public ListViewPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;//启用页面缓存
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)//导航至此页面方法
        {
            base.OnNavigatedTo(e);
            //refresh.Height = 0;
            if (MainPage.frame.IoN)
            {
                GetListContent();
            }
        }
        public async void GetListContent()//获取list内容
        {
            string content = await NetWork.NetWork.NetWorks(api);
            string json = GetItem(content);
            list = JsonConvert.DeserializeObject<ObservableCollection<Contentlist>>(json);
            ListView.ItemsSource = list;
        }
        public string GetItem(string content)//获取详细内容
        {
            string _content = "";
            JObject jobject = JObject.Parse(content);
            JToken json0 = jobject["showapi_res_body"];
            JToken json1 = json0["pagebean"];
            JToken json2 = json1["contentlist"];
            _content = json2.ToString();
            return _content;
        }
        public bool isLoading = false;
        private object o = new object();
        public void Refresh(ListViewBase sender, ContainerContentChangingEventArgs args)
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
                                string content = await NetWork.NetWork.NetWorks(api);
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

        double offset = 0.0;
        private void ListView_ItemClick(object sender, ItemClickEventArgs e)//listitem点击事件
        {
            offset = ScrollRoot.VerticalOffset;
            MainPage.frame.IoN = false;
            Contentlist c = (Contentlist)e.ClickedItem;
            MainPage.frame.ContentFrame.Navigate(typeof(PlayPage), c);
        }
        public async void LoadMoreItemsAsync(int count)//底部加载更多方法
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

        public bool _isPullRefresh = false;//下拉刷新1
        public event PropertyChangedEventHandler PropertyChanged;//下拉刷新2
        public bool IsPullRefresh//下拉刷新3
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
        public void OnPropertyChanged(string name)//下拉刷新4
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private int count = 1;
        private async void ScrollRoot_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)//滚动条事件
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
                    IsPullRefresh = true;//下拉刷新
                    string content = await NetWork.NetWork.NetWorks(api);
                    string json = GetItem(content);
                    list = JsonConvert.DeserializeObject<ObservableCollection<Contentlist>>(json);
                    ListView.ItemsSource = list;
                    sv.ChangeView(null, 50, null);
                }
                IsPullRefresh = false;
            }

        }

        private void hateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void downLoadButton_Click(object sender, RoutedEventArgs e)//下载按钮事件
        {
            AppBarButton down = sender as AppBarButton;
            var s = down.DataContext as Contentlist;
            StorageFile file = await NetWork.DownLoad.DownLoadItem(s.video_uri);
            Debug.WriteLine(file);
            MainPage.frame.title.Text = "Video";
        }

        private void ScrollRoot_Loaded(object sender, RoutedEventArgs e)//滚动条加载事件
        {
            ScrollRoot.ChangeView(null, 50, null);
        }
    }
}
