using Newtonsoft.Json;
using System.Text;

namespace Translator
{
    public class Program
    {
        private static readonly string key = "9e8ad32e8c1b4c32b56776fb92a85f48";
        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com/";
        private static readonly string region = "westeurope";

        public static async Task Main()
        {
            Console.Write("Din Text: ");
            var textToTranslate = Console.ReadLine();


            var json = JsonConvert.SerializeObject(new object[] { new { Text = textToTranslate } });

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    request.RequestUri = new Uri(endpoint + "/translate?api-version=3.0&to=en");
                    request.Method = HttpMethod.Post;
                    request.Headers.Add("Ocp-Apim-Subscription-Key", key);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", region);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");


                    var response = await client.SendAsync(request);
                    var result = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                    Console.WriteLine(result[0].translations[0].text);
                }
            }

            Console.ReadKey();
        }

    }
}
