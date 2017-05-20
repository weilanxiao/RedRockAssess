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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace RedrockAssess.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListViewPage : Page
    {
        ObservableCollection<Contentlist> list = new ObservableCollection<Contentlist>();
        ObservableCollection<Contentlist> list1 = new ObservableCollection<Contentlist>();
        private string api = @"http://route.showapi.com/255-1?showapi_appid=38525&showapi_sign=b4a7ceb202cf4eba9abd8041b01b7b31&type=41&page=1";
        public ListViewPage()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string content = await NetWork.NetWork.NetWorks(api);
            string json = GetItem(content);
            //string image = GetImage(json);
            list = JsonConvert.DeserializeObject<ObservableCollection<Contentlist>>(json);
            ListView.ItemsSource = list;
        }
        public async void LoadMoreItemsAsync(uint count)
        {
            string _count = count.ToString();
            _count = "page=" + _count;            
            string content = await NetWork.NetWork.NetWorks(api.Replace("page=1", _count));
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
            Debug.WriteLine(json2);
            return _content;
        }
        //public string GetImage(string json)
        //{
        //    JToken jobect = JToken.Parse(json);
        //    string profile_image = jobect["profile_image"].ToString();
        //    Debug.WriteLine(profile_image);
        //    return profile_image;
        //}
        //public void GetVisual()
        //{
        //    ListView;
        //}
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
            sv = FindVisualChildByName<ScrollViewer>(list, "ScrollViewer");
            sb = FindVisualChildByName<ScrollBar>(sv, "VerticalScrollBar");
            sb.ValueChanged += Sb_ValueChanged;
        }
        public async void A()
        {
            string _count = "2";
            _count = "page=" + _count;
            string content = await NetWork.NetWork.NetWorks(api.Replace("page=1", _count));
            string json = GetItem(content);
            list1 = JsonConvert.DeserializeObject<ObservableCollection<Contentlist>>(json);
            ListView.ScrollIntoView(list1);
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

        private double _height;
        private void ScrollRoot_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (ScrollRoot.VerticalOffset == ScrollRoot.ScrollableHeight)
            {
                LoadMoreItemsAsync(2);//滚动条到底部
            }
            else
            {
                //_height = ScrollRoot.VerticalOffset;
            }            

            //if (_isLoding) return;
            //if (ScrollRoot.VerticalOffset <= ScrollRoot.ScrollableHeight - 500) return;
            //if (_currentPage >= _countPage + 1) return;

            //_isLoding = true;

        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
