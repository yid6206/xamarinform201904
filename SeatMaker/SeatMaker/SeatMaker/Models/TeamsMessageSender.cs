using Newtonsoft.Json;
using System;
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

        public async Task<bool> SendAsync(string body) => await SendAsync(body, null, null);
        public async Task<bool> SendAsync(string body, string title) => await SendAsync(body, title, null);
        public async Task<bool> SendAsync(string body, string title, string colorCode)
        {
            return await PostAsync(_url, CreateModel(body, title, colorCode));
        }

        private TeamsWebhookModel CreateModel(string body, string title, string colorCode)
        {
            return new TeamsWebhookModel
            {
                ThemeColor = colorCode,
                Summary = "C# .Net Standard",
                Title = title,
                Text = body,
                Sections = new List<Section>()
            };
        }

        private async Task<bool> PostAsync(string url, TeamsWebhookModel model)
        {
            using (var httpClient = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json);

                var response = await httpClient.PostAsync(url, content);
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
    }
}
