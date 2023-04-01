using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortLibrary
{
    /// <summary>
    /// Lớp khởi tạo tham số cho cổng kết nối
    /// </summary>
    public class SerialConfig
    {
        public SerialConfig() { }
        public SerialConfig(string _portName, int _baudRate)
        {
            PortName = _portName;
            BaudRate = _baudRate;
        }
        /// <summary>
        /// Tên cổng truyền vào
        /// </summary>
        private string _portName = "COM2";
        public string PortName
        {
            get => _portName;
            set => _portName = value;
        }
        /// <summary>
        /// Tốc độ truyền
        /// </summary>
        private int _baudRate = 4800;
        public int BaudRate
        {
            get => _baudRate;
            set => _baudRate = value;
        }

    }
}
