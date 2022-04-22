using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs;

namespace AzureFunctions
{
    public class FlushIotHubQueue
    {
        private static HttpClient client = new HttpClient();
        
        [FunctionName("FlushIotHubQueue")]
        public void Run([IoTHubTrigger("messages/events", Connection = "IotHubEndpoint", ConsumerGroup = "storage")]EventData message, ILogger log)
        {
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.EventBody)}");
        }
    }
}