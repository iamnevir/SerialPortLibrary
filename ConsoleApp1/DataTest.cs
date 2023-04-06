using SerialPortLibrary;
using System.Text;
namespace Test
{
    public static class DataTest
    {
        public static void ReadTest1()
        {
            Console.OutputEncoding = Encoding.UTF8;
            SerialConfig serialConfig = new() { PortName = "COM3" };
            SerialPortLb serialPortLb = new(serialConfig);
            byte[] bytes = new byte[]{ 0x01, 0x03, 0x00, 0x00, 0x00, 0x01, 0x84, 0x0A };
            serialPortLb.Read(bytes,0,bytes.Length);
        }
        public static void ReadTest2()
        {
            Console.OutputEncoding = Encoding.UTF8;
            SerialConfig serialConfig = new() { PortName = "COM3" };
            SerialPortLb serialPortLb = new(serialConfig);
            char[] bytes = new char[] { (char)0x01 };
            serialPortLb.Read(bytes, 0, bytes.Length);
        }
    }
}
