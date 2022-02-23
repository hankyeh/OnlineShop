using System.Net;
using System.Text;
using System.Text.Json;

namespace ShopBackend.UTL
{
    public class WebApiConn
    {
        /// <summary>
        /// CALL GET API
        /// </summary>
        /// <typeparam name="TValue">取回資料型態</typeparam>
        /// <param name="url">api 路徑</param>
        /// <returns></returns>
        public TValue GetApi<TValue>(string url)
        {
            TValue Result;
            string targetUrl = url;

            HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.Accept = "application/json";
            request.Timeout = 30000;

            string result = "";
            // 取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    //不分大小寫
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Result = JsonSerializer.Deserialize<TValue>(result, options);
                }
            }
            return Result;
        }

        /// <summary>
        /// CALL POST API
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public TValue PostApi<TValue>(string url, string importData)
        {
            TValue Result;
            string targetUrl = url;
            //string parame = "p=Arvin";
            string parame = importData;//"{\"id\": 0, \"name\": \"PlayStaion 6 主機\", \"products\": []}";
            byte[] postData = Encoding.UTF8.GetBytes(parame);

            HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
            request.Method = "POST";
            request.Accept = "text/plain";
            request.ContentType = "application/json";
            request.Timeout = 30000;
            request.ContentLength = postData.Length;
            request.AllowAutoRedirect = false;  // 禁止重新導向網頁
            // 寫入 Post Body Message 資料流
            using (Stream st = request.GetRequestStream())
            {
                st.Write(postData, 0, postData.Length);
            }

            string result = "";
            // 取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    //不分大小寫
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    Result = JsonSerializer.Deserialize<TValue>(result, options);
                }
            }

            return Result;
        }

        /// <summary>
        /// CALL PUT API
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public HttpStatusCode PutApi(string url, string importData)
        {
            string targetUrl = url;
            //string parame = "p=Arvin";
            string parame = importData;//"{\"id\": 0, \"name\": \"PlayStaion 6 主機\", \"products\": []}";
            byte[] postData = Encoding.UTF8.GetBytes(parame);

            HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
            request.Method = "PUT";
            request.Accept = "text/plain";
            request.ContentType = "application/json";
            request.Timeout = 30000;
            request.ContentLength = postData.Length;
            request.AllowAutoRedirect = false;  // 禁止重新導向網頁
            // 寫入 Post Body Message 資料流
            using (Stream st = request.GetRequestStream())
            {
                st.Write(postData, 0, postData.Length);
            }

            string result = "";
            // 取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                return response.StatusCode;
        }

        /// <summary>
        /// CALL DELETE API 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public HttpStatusCode DeleteApi(string url)
        {
            string targetUrl = url;

            HttpWebRequest request = HttpWebRequest.Create(targetUrl) as HttpWebRequest;
            request.Method = "DELETE";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = 30000;

            string result = "";
            // 取得回應資料
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                return response.StatusCode;

        }
    }
}
