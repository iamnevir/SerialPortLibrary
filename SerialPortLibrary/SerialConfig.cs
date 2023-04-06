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
        public SerialConfig(string _portName)
        {
            PortName = _portName;
        }
        public SerialConfig(string _portName, int _baudRate)
        {
            PortName = _portName;
            BaudRate = _baudRate;
        }       
        public SerialConfig(string _portName, int _baudRate, Parity _parity, int _writeTimeOut, int _readTimeOut)
        {
            PortName = _portName;
            BaudRate = _baudRate;
            Parity = _parity;
        }
        public SerialConfig(string _portName, int _baudRate, Parity _parity, int _dataBits, int _writeTimeOut, int _readTimeOut)
        {
            PortName = _portName;
            BaudRate = _baudRate;
            Parity = _parity;
            DataBits = _dataBits;
        }
        public SerialConfig(string _portName, int _baudRate, Parity _parity, int _dataBits, StopBits _stopBits, int _writeTimeOut, int _readTimeOut)
        {
            PortName = _portName;
            BaudRate = _baudRate;
            Parity = _parity;
            DataBits = _dataBits;
            StopBits = _stopBits;
        }
        public SerialConfig(string _portName, int _baudRate, Parity _parity, int _dataBits, StopBits _stopBits, Handshake _handshake, int _writeTimeOut, int _readTimeOut)
        {
            PortName = _portName;
            BaudRate = _baudRate;
            Parity = _parity;
            DataBits = _dataBits;
            StopBits = _stopBits;
            Handshake = _handshake;
            WriteTimeOut = _writeTimeOut;
            ReadTimeOut = _readTimeOut;
        }
        private string _portName = "COM2";
        /// <summary>
        /// Tên cổng truyền vào
        /// </summary>
        public string PortName
        {
            get => _portName;
            set => _portName = value;
        }

        private int _baudRate = 4800;
        /// <summary>
        /// Tốc độ truyền
        /// </summary>
        public int BaudRate
        {
            get => _baudRate;
            set => _baudRate = value;
        }

        private Parity _parity = Parity.None;
        /// <summary>
        /// Nhận hoặc đặt giao thức kiểm tra chẵn lẻ.
        /// </summary>
        public Parity Parity
        {
            get => _parity;
            set => _parity = value;
        }

        private StopBits _stopBits = StopBits.One;
        /// <summary>
        ///  Nhận hoặc đặt số lượng bit dừng tiêu chuẩn trên mỗi byte  
        /// </summary>
        public StopBits StopBits
        {
            get => _stopBits;
            set => _stopBits = value;
        }

        private Handshake _handshake = Handshake.None;
        /// <summary>
        /// Nhận hoặc đặt giao thức bắt tay để truyền dữ liệu qua cổng nối tiếp bằng cách sử dụng một giá trị từ Bắt tay .
        /// </summary>
        public Handshake Handshake
        {
            get => _handshake;
            set => _handshake = value;
        }

        private int _dataBits = 8;
        /// <summary>
        /// Nhận hoặc đặt độ dài tiêu chuẩn của bit dữ liệu trên mỗi byte.
        /// </summary>
        public int DataBits
        {
            get => _dataBits;
            set => _dataBits = value;
        }

        private int _readTimeOut = 500;
        /// <summary>
        /// Nhận hoặc đặt số mili giây trước khi hết thời gian chờ khi thao tác đọc không kết thúc.
        /// </summary>
        public int ReadTimeOut
        {
            get => _readTimeOut;
            set => _readTimeOut = value;
        }

        private int _writeTimeOut = 500;
        /// <summary>
        /// Nhận hoặc đặt số mili giây trước khi hết thời gian chờ khi thao tác ghi không kết thúc.
        /// </summary>
        public int WriteTimeOut
        {
            get => _writeTimeOut;
            set => _writeTimeOut = value;
        }


    }
}
