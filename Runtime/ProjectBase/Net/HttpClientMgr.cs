using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace Momos.Core.Net {
    public class HttpClientMgr : BaseManager<HttpClientMgr> {
        HttpClient client = new HttpClient();

        /// <summary> await HttpClientMgr.GetInstance().DownloadRes(url, dp); </summary>
        /// <returns> 状态码,使用if(<see cref="response.IsSuccessStatusCode"/>) </returns>
        public async Task<HttpStatusCode> DownloadRes(string url,string downloadPath) {
            var request = new HttpRequestMessage(HttpMethod.Head, url);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) {
                byte[] data = await client.GetByteArrayAsync(url);
                using FileStream stream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
                await stream.WriteAsync(data, 0, data.Length);
                
            }
            return response.StatusCode;
            //Debug.LogError($"请求失败: {url}, 状态码: {response.StatusCode}");
        }
    }
}