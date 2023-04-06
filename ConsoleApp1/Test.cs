using SerialPortLibrary;
using System.Text;

namespace Test;

    public class Test
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            SerialConfig serialConfig = new() { PortName = "COM3" };
            byte[] a = { 0x01, 0x03, 0x00, 0x00, 0x00, 0x01, 0x84, 0x0A };
            DataSend dataSend = new(a);
            SerialPortLb serialPortLb = new(serialConfig);
            Sensor sensor = new(serialPortLb)
            {
                Message = (a) => Console.WriteLine(a)
            };
            sensor.Run(dataSend.Data);
        }
    }
