using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Web.Http;

namespace RedrockAssess.NetWork
{
    class DownLoad
    {
        public static async Task<StorageFile> DownLoadItem(string uri)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var buffer = await httpClient.GetBufferAsync(new Uri(uri));
                if (buffer != null && buffer.Length > 0)
                {
                    string []str=uri.Split('/');
                    string filename = str[str.Length - 1];
                    var file =await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(filename);
                    using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await stream.WriteAsync(buffer);
                        await stream.FlushAsync();
                    }
                    return file;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }
    }
}
