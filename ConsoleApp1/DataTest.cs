using SerialPortLibrary;
using System.Text;
namespace Test
{
    public static class DataTest
    {
        public static void ReadTest1()
        {
            Console.OutputEncoding = Encoding.UTF8;
            SerialConfig serialConfig = new("COM3");
            SerialPortLb serialPortLb = new(serialConfig);
            byte[] bytes = new byte[8];
            serialPortLb.Read(bytes,0,bytes.Length);
        }
        public static void ReadTest2()
        {
            Console.OutputEncoding = Encoding.UTF8;
            SerialConfig serialConfig = new("COM3") ;
            SerialPortLb serialPortLb = new(serialConfig);
            char[] bytes = new char[8];
            serialPortLb.Read(bytes, 0, bytes.Length);
        }
    }
}
