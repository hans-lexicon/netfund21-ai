using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

namespace IotDevice_Console
{
    public class Program
    {
        // HTTPS, MQTT, AMQP
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=netfund21-iothub.azure-devices.net;DeviceId=console;SharedAccessKey=YvxOEWbXwV7fNsD5xSqXSgkawTeKMgo+yxHPIhoY2Pw=", TransportType.Mqtt);
        private static readonly Random rnd = new Random();
        private static int interval = 10000;
        private static bool messageSending = true;
        private static string deviceId = "console";
        private static int messageId = 1;
        private static int messageCount = 0;

        public static async Task Main()
        {
            while(true)
            {
                var temperature = rnd.Next(8, 20);
                var humidity = rnd.Next(30, 60);

                if(messageSending)
                {
                    if(messageCount == 20)
                    {
                        temperature = 0;
                        humidity = 0;
                        messageCount = 0;
                    }

                    var messagePayload = JsonConvert.SerializeObject(new { deviceId = deviceId, messageId = messageId++, temperature = temperature, humidity = humidity });
                    var message = new Message(Encoding.UTF8.GetBytes(messagePayload));
                    Console.WriteLine(messagePayload);

                    if(temperature < 10)
                        message.Properties["temperatureAlert"] = "true";
                    else
                        message.Properties["temperatureAlert"] = "false";

                    await deviceClient.SendEventAsync(message);

                    messageCount++;
                }

                await Task.Delay(interval);
            }
        }





    }
}