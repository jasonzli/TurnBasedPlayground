using System.Net.Http;
using System.Threading.Tasks;

namespace Code.Utility
{
    /// <summary>
    /// Some URL utility functions for JSON
    /// </summary>
    public static class URLUtility
    {
        public static async Task<string> FetchJSONStringFromURL(string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(url);
                }
                catch
                {
                    return null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
    
}