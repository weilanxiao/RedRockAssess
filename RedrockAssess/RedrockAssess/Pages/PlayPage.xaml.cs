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
    public sealed partial class PlayPage : Page
    {
        public PlayPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string p = "";
            if(e.Parameter is Cache)//缓存视频播放
            {
                Cache x = e.Parameter as Cache;
                p = x.path;
            }else if(e.Parameter is Contentlist)//在线视频播放
            {
                Contentlist x = e.Parameter as Contentlist;
                p = x.video_uri;
                List<Cache> list = new List<Cache>();
                list = CachePage.cachePage.CacheList.ItemsSource as List<Cache>;
                foreach (Cache cs in list)
                {
                    if (p.Contains(cs.name))//在线视频缓存播放
                    {
                        p = cs.path;
                        Debug.WriteLine(p);
                        MainPage.frame.title.Text = "Cache";
                    }
                }                
            }
            else
            {
                Debug.WriteLine("未知类型！");
            }
            Play(p);

        }
        public  void Play(string path)//播放视频方法
        {
            try
            {
                MediaElement m = new MediaElement();
                m.Source = new Uri(path);
                m.AreTransportControlsEnabled = true;
                m.TransportControls.IsCompact = false;
                m.AutoPlay = true;
                m.TransportControls.IsSeekBarVisible = true;                
                play.Children.Add(m);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
