using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SeatMaker.Models
{
    public class TeamsMessageSender
    {
        private readonly string _url;

        public TeamsMessageSender(string url) => _url = url;

        public async Task<bool> SendImageAsync(string message, string imageUrl)
        {
            var model = new Hashtable();
            model["Text"] = $"{message}\n[{imageUrl}]({imageUrl})";
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json);

                var response = await httpClient.PostAsync(_url, content);
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}
