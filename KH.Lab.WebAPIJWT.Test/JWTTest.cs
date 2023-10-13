using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace KH.Lab.WebAPIJWT.Test
{
    public class JWTTest
    {
        [Fact]
        public void LoginTest()
        {
            //API的URL
            string url = "https://localhost:7059/api/authentication/login";

            //建立傳遞參數
            var loginModel = new { UserName = "admin", Password = "admin" };

            //將loginModel序列化為Jason字串
            var jsonString = JsonConvert.SerializeObject(loginModel);

            //非同步取得 HTTP Content
            string responseResult = Send(url, "", jsonString);

            //將responseResult字串轉為物件
            dynamic jobj = JsonConvert.DeserializeObject(responseResult);

            // 取得 Token
            string token = jobj.data.token;

            //API的URL
            url = "https://localhost:7059/api/shipment/getbyno";

            //建立傳遞參數
            var param = new { BatchNo = "DBN20231012140649844" };

            //將param序列化為Jason字串
            jsonString = JsonConvert.SerializeObject(param);

            //非同步取得 HTTP Content
            string responseData = Send(url, token, jsonString);

            //將responseResult字串轉為物件
            jobj = JsonConvert.DeserializeObject(responseData);

            // 取得資料
            var data = jobj.data;

            // 比對 Token
            Assert.True(!string.IsNullOrEmpty(token));

            // 比對 Data
            Assert.True(data != null);
        }

        /// <summary>
        /// 傳送
        /// </summary>
        /// <param name="url">請求網址</param>
        /// <param name="authorization">JWT Token</param>
        /// <param name="content">內容</param>
        /// <returns></returns>
        public string Send(string url, string authorization, string content)
        {
            //宣告HttpRequestMessage ( HttpMethod=Post，requestUri=url )
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            //HttpContent內容 ( content=jsonString, Encoding=UTF8, mediaType ="application/json")
            request.Content = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
            //判斷用戶端具名
            using var client = new HttpClient();
            //逾時設定30秒
            client.Timeout = TimeSpan.FromSeconds(30);
            //設定Header Authorization
            if (!string.IsNullOrWhiteSpace(authorization))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorization);
            }
            //送出Request，並接收HttpResponseMessage
            HttpResponseMessage response = client.SendAsync(request).Result;
            //非同步取得 HTTP Content
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
