using Pharmacies.Data;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

namespace Pharmacies
{
    public class PostiitClient
    {
        IConfiguration _config;
        public PostiitClient(IConfiguration config)
        {
            _config = config;

        }
        public string GetPostCode(string address)
        {
            //TODO: need error handling
            var httpClient = new HttpClient();

            var url = _config["postitUrl"]; 
            var key = _config["postitKey"]; 
            var respone = httpClient.GetAsync($@"{url}/?term={address}&key={key}").GetAwaiter().GetResult();
            respone.EnsureSuccessStatusCode();
            var responseString = respone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var postCode = JsonSerializer.Deserialize<PostitResponse>(responseString).data.First().post_code;
            return postCode;
        }

    }
    public class PostitiOptions
    {
        public string url { get; set; }
        public string key { get; set; }
    }
    public class AddressData
    {
        public string post_code { get; set; }
    }

    public class PostitResponse
    {
        public string status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public int message_code { get; set; }
        public int total { get; set; }
        public List<AddressData> data { get; set; }
    }
}
