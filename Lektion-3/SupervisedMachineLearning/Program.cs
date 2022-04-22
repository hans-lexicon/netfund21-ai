using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CallRequestResponseService
{
    class Program
    {
        private static readonly string apiKey = "vZWXVxebQ8RxPBlFzo/5ogWWFTf/pGryxueHHq1lO83qyNi5qvshF483fA9/G259u0pD2eEJ0FRtgsHUaciUCg==";
        private static readonly string apiUri = "https://ussouthcentral.services.azureml.net/workspaces/713ead5c2bfe447d8c1bdd33c5cfbe4c/services/6b2aacf845ff457487b80283f0d133da/execute?api-version=2.0&details=true";
        static async Task Main(string[] args)
        {
            Console.Write("Enter temperature (°C): ");
            var temperature = Console.ReadLine() ?? "0";

            Console.Write("Enter humidity (%): ");
            var humidity = Console.ReadLine() ?? "0";

            await InvokeRequestResponseService(temperature, humidity);
        }      


        static async Task InvokeRequestResponseService(string temperature, string humidity)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var scoreRequest = "{\"Inputs\": {\"input1\": {\"ColumnNames\": [\"temperature\",\"humidity\"],\"Values\": [[\"" + temperature + "\",\"" + humidity + "\"]]}},\"GlobalParameters\": { }}";
            HttpResponseMessage response = await client.PostAsync(apiUri, new StringContent(scoreRequest, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(result);
                Console.WriteLine("Chance of Rain: " + data?.Results.output1.value.Values[0][2]);
                Console.WriteLine("Score in Procent (%): " + data?.Results.output1.value.Values[0][3]);
                Console.WriteLine("\n{0}", result);
            }
            else
            {
                Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));
            }
        }
    }
}
