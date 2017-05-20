using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace RedrockAssess.NetWork
{
    class NetWork
    {
        private string api = @"http://route.showapi.com/255-1?showapi_appid=38525&showapi_sign=b4a7ceb202cf4eba9abd8041b01b7b31&type=41";
        public static async Task<string> NetWorks(string uri)
        {
            string content = "";
            try
            {
                return await Task.Run(() =>
                {
                    HttpClient httpClient = new HttpClient();
                    HttpResponseMessage httpRespone = httpClient.GetAsync(uri).Result;
                    if (httpRespone.StatusCode != System.Net.HttpStatusCode.OK)
                        Debug.WriteLine(httpRespone.StatusCode);
                    return content = httpRespone.Content.ReadAsStringAsync().Result;
                });
            }
            catch (Exception ex)
            {
                return content = "请求异常！";
                Debug.WriteLine(content);
            }

        }
    }
}
