using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

namespace IotDevice
{
    public class Program
    {
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=netfund21-iothub.azure-devices.net;DeviceId=tempdevice;SharedAccessKey=9udUhkOTbDvV6O4i2oQrPt6ZDkV+QtUkhgSD59JXT3I=", TransportType.Mqtt);

        public static async Task Main()
        {
            Console.WriteLine("IOT Device is Running...");
            await SendMessageAsync();
            Console.ReadKey();
        }

        private static async Task SendMessageAsync()
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
            var rnd = new Random();
            var temperature = rnd.Next(10, 60);
            var humidity = rnd.Next(10, 90);

            do
            {
                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { temperature, humidity })));
                await deviceClient.SendEventAsync(message);

            } while (await timer.WaitForNextTickAsync());
        }
    }
}
