using SerialPortLibrary;
using System.Text;

namespace Test;

public class Test
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        var configManager = ConfigManager<SerialConfig>.Instance.Config;
        SerialConfig serialConfig = new();
        byte[] a = { 0x01, 0x03, 0x00, 0x00, 0x00, 0x01, 0x84, 0x0A };
        byte[] b = new byte[10];
        DataSend dataSend = new(a);
        SerialPortLb serialPortLb = new(configManager);
        Sensor sensor = new(serialPortLb)
        {
            Message = (a) => Console.WriteLine(a)
        };
        bool _continue = true;
        while (_continue)
        {
            string text = Console.ReadLine();
            switch (text)
            {
                case "read":
                    sensor.Run(a, b);
                    break;
                case "readph":
                    sensor.Run(a);
                    break;
            }
        }

    }
}
