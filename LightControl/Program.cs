using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YeelightAPI;
using YeelightAPI.Models;

namespace LightControl
{
    class Program
    {
        public static void Main(string[] args)
        {
            var devices = Discover().Result;
            if (devices != null && devices.Count == 1)
            {
                var device = devices[0];
                Connect(device);
                while(true)
                    Menu(device);
            }
            else if (devices == null)
                Console.WriteLine("There were no devices found");
            else
                Console.WriteLine("There were multiple devices found");

            Console.ReadKey();
        }

        public static async Task<List<Device>> Discover()
        {
            return await DeviceLocator.Discover();
        }

        public static async void Connect(Device device)
        {
            if (!device.IsConnected)
                await device.Connect();
        }

        public static void Menu(Device device)
        {
            bool success;
            int result;
            do
            {
                Console.WriteLine($"1. Turn on\n2. Turn off\n 3. Change brightness\n");
                success = int.TryParse(Console.ReadLine(), out result);
            }
            while (!success || result < 1 || result > 3);

            switch (result)
            {
                case 1:
                    TurnOn(device);
                    break;
                case 2:
                    TurnOff(device);
                    break;
                case 3:
                    ChangeBrightness(device);
                    break;
            }
        }

        public static async void TurnOn(Device device)
        {
            if (!(device is IDeviceReader deviceReader)) return;
            var power = (await deviceReader.GetProp(PROPERTIES.power)).ToString();
            if (power=="on")
                Console.WriteLine("Power already on!");
            else
                await device.SetPower();
        }

        public static async void TurnOff(Device device)
        {
            if (!(device is IDeviceReader deviceReader)) return;
            var power = (await deviceReader.GetProp(PROPERTIES.power)).ToString();
            if (power == "off")
                Console.WriteLine("Power already off!");
            else
                await device.SetPower(false);
        }

        public static async void ChangeBrightness(Device device)
        {
            Console.WriteLine("\nInput targeted brightness level: ");
            var success = int.TryParse(Console.ReadLine(), out var brightness);
            if (!success || brightness < 1 || brightness > 100)
            {
                Console.WriteLine("Invalid input!");
                return;
            }
            await device.SetBrightness(brightness);
        }
    }
}
