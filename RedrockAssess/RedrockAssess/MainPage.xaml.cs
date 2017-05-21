using System;
using System.Collections.Generic;
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
using RedrockAssess.Pages;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace RedrockAssess
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage frame;

        public bool IoN = true;//后退刷新标识
        public MainPage()
        {
            this.InitializeComponent();
            frame = this;
            title.Text = "Cache";
            ContentFrame.Navigate(typeof(CachePage));
        }

        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ListViewPage));
        }

        private void CacheButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(CachePage));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void IconsListBos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (First.IsSelected)//选择该项
            {
                //ReturnButton.Visibility = Visibility.Visible;//设置可见性
                ContentFrame.Navigate(typeof(ListViewPage));//跳转到该页
                title.Text = "Video";
            }
            else if (Second.IsSelected)
            {
                ContentFrame.Navigate(typeof(CachePage));
                title.Text = "Cache";
            }else if (Third.IsSelected)
            {
                ContentFrame.Navigate(typeof(HistoryPage));
                title.Text = "History";
            }
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }
    }
}
