using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Text;

namespace IotDevices
{
    public static class Program
    {
        private static Random rnd = new Random();
        private static List<string> devices = new List<string>()
        {
            "HostName=netfund21-iothub.azure-devices.net;DeviceId=d1;SharedAccessKey=HUUVMC2hw06MbeEKBot5owJxBrEZdGFDSHXwpJzQzic=",
            "HostName=netfund21-iothub.azure-devices.net;DeviceId=d2;SharedAccessKey=MZFsD7JuO08lL3pN8D2uR03lG36MGgc4hjoq0RpiQ9o=",
            "HostName=netfund21-iothub.azure-devices.net;DeviceId=d3;SharedAccessKey=U7xRxiSFFWBRuJD7Wb/gz6UT0AxvrvTbUNhtRI24Gvw=",
            "HostName=netfund21-iothub.azure-devices.net;DeviceId=d4;SharedAccessKey=gt2ZaFCugUHt7IX2NZJZT6FZ2/Lh7igly/TfNOdACcY=",
            "HostName=netfund21-iothub.azure-devices.net;DeviceId=d5;SharedAccessKey=PIRBUq+cKHXcLZekiERl6k7TWG3g9WmG+6abNgstdjE="
        };

        private static async Task SendAsync(string deviceId, DeviceClient deviceClient)
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(rnd.Next(5, 15)));
            var counter = 0;
            var temperature = 0;

            do
            {


                if (counter == 20)
                {
                    temperature = rnd.Next(100,200);
                    counter = 0;
                } 
                else
                {
                    temperature = rnd.Next(22, 24);
                }
                counter++;

                var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new { deviceId, temperature })));
                await deviceClient.SendEventAsync(message);
                Console.WriteLine($"{deviceId} sent a message at: {DateTime.Now} with temperature: {temperature}");

            } while (await timer.WaitForNextTickAsync());

        }

        private static async Task InitAsync(string connectionstring)
        {
            var deviceId = connectionstring.Split(";")[1].Split("=")[1];
            var deviceClient = DeviceClient.CreateFromConnectionString(connectionstring, TransportType.Mqtt);
            Console.WriteLine($"{deviceId} initialized.");

            await SendAsync(deviceId, deviceClient);
        }


        public static void Main()
        {
            foreach (var device in devices)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(rnd.Next(3000, 10000));
                    await InitAsync(device);
                });             
            }
                

            Console.ReadKey();
        }
    }
}