using RedrockAssess.Model;
using System;
using System.Collections.Generic;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace RedrockAssess.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CachePage : Page
    {
        public static CachePage cachePage;
        public CachePage()
        {
            this.InitializeComponent();
            cachePage = this;
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            GetPath();
        }
        List<Cache> list = new List<Cache>();
        string _path = @"C:\Users\GZ\AppData\Local\Packages\9788a00b-1c3d-4a8e-9df0-9aef946002c8_pdv0hatz6g5vg\LocalCache\";
        public void GetPath()
        {
            var files = Directory.GetFiles(_path);
            foreach (var a in files)
            {
                Cache x = new Cache();
                x.path = a;
                x.name = x.path.Replace(_path, "");
                list.Add(x);
            }
            CacheList.ItemsSource = list;
        }

        private void CathList_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cache c = e.ClickedItem as Cache;
            MainPage.frame.ContentFrame.Navigate(typeof(PlayPage), c);
        }
    }
}
