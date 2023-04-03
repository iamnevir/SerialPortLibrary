using SerialPortLibrary;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        while (true)
        {
            Console.WriteLine("Request>");
            string request = Console.ReadLine();
            switch (request.ToLower())
            {
                case "run":
                    SerialConfig serialConfig = new() { PortName="COM3"};
                    Sensor sensor = new(serialConfig, 500, 500);
                    sensor.Message = (s) => Console.WriteLine(s);
                    _ = sensor.Run();

                    break;
                case "load":
                    Sensor.Load();
                    break;
                default:
                    Console.WriteLine("Command Not Found!");
                    break;
            }
        }
    }
    
}
