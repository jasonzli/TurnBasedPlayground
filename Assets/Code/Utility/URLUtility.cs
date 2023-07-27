using System.Net.Http;
using System.Threading.Tasks;
using Code.BattleSystem;
using Code.ScriptableObjects;
using Newtonsoft.Json;

namespace Code.Utility
{
    public static class URLUtility
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