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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace RedrockAssess.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ListViewPage : Page
    {
        ObservableCollection<Contentlist> list = new ObservableCollection<Contentlist>();
        private string api = @"http://route.showapi.com/255-1?showapi_appid=38525&showapi_sign=b4a7ceb202cf4eba9abd8041b01b7b31&type=41";
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
    }
}
