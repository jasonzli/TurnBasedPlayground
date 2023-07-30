using System.Net.Http;
using System.Threading.Tasks;

namespace Code.Utility
{
    /// <summary>
    /// Some URL utility functions for JSON
    /// </summary>
    public static class URLUtility
    {
        /// <summary>
        /// There are a few kinds of errors that can come from these awaits. We try our best to catch them here before something else happens
        /// </summary>
        /// <param name="url">the url to ping</param>
        /// <returns>url response</returns>
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