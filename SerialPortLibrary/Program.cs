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
        while (true)
        {
            Console.WriteLine("Request>");
            string request = Console.ReadLine();
            switch (request.ToLower())
            {
                case "run":
                    SerialConfig serialConfig = new SerialConfig();
                    Senior senior = new Senior(serialConfig, 500, 500);
                    senior.Run();
                        break;
                default:
                    Console.WriteLine("Command Not Found!");
                    break;
            }
        }
    }

}