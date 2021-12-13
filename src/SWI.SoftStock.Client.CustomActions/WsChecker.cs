using System.IO;
using System.Net;

namespace SWI.SoftStock.Client.CustomActions
{
    public class WsChecker
    {
        public int Check(CheckRequest request)
        {
            var requestUrl = $"{request.ServiceAddress}/{request.UniqueCompanyId}";
            var http = (HttpWebRequest)WebRequest.Create(requestUrl);
            http.Accept = "application/json";
            http.Method = "GET";

            using (var response = http.GetResponse())
            {
                var responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    var reader = new StreamReader(responseStream);
                    var receiveContent = reader.ReadToEnd();
                    reader.Close();
                    return int.TryParse(receiveContent, out var result) ? result : 1;
                }
            }

            return 1;
        }
    }
}