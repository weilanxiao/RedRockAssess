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
using RedrockAssess.Model;
using SQLite;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace RedrockAssess.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HistoryPage : Page
    {
        public SQLiteAsyncConnection MyConnection { get; set; }
        List<HistoryModel> list = new List<HistoryModel>();
        public static HistoryPage historyPage;
        public HistoryPage()
        {
            this.InitializeComponent();
            historyPage = this;
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.frame.Third.IsSelected = true;
            RefreshSQL();
        }

        private void HistoryList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var x = e.ClickedItem as HistoryModel;
            MainPage.frame.ContentFrame.Navigate(typeof(PlayPage),x);
        }
        public async void InsertSQL(HistoryModel history)//插入数据库
        {
            MyConnection = new SQLiteAsyncConnection("HistoryModel.db");
            await MyConnection.CreateTableAsync<HistoryModel>();
            await MyConnection.InsertAsync(history);
            var query = await (MyConnection.Table<HistoryModel>().Where(v => v._ID >= 1)).ToListAsync();
            list = new List<HistoryModel>(query);
            HistoryList.ItemsSource = list;
        }
        public async void RefreshSQL()//刷新数据库
        {
            MyConnection = new SQLiteAsyncConnection("HistoryModel.db");
            await MyConnection.CreateTableAsync<HistoryModel>();
            var query = await (MyConnection.Table<HistoryModel>().Where(
                v => v._ID >= 1).ToListAsync());
            list = new List<HistoryModel>(query);
            HistoryList.ItemsSource = list;
        }

    }
}
