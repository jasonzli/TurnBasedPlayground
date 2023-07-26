using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Code
{
    public static class URLFetch
    {
        public static async Task<string> FetchJSONStringFromURL(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<T> FetchJSONFromURL<T>(string url)
        {
            string jsonString = await FetchJSONStringFromURL(url);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}