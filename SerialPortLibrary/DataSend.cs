using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortLibrary
{
    /// <summary>
    /// Lớp chứa dữ liệu về gói tin gửi đến cổng kết nối
    /// </summary>
    public static class DataSend
    {
        public static byte[] PhData = { 0x01, 0x03, 0x00, 0x00, 0x00, 0x01, 0x84, 0x0A };
        public static byte[] data = { 0x02, 0x03, 0x00, 0x01, 0x00, 0x01, 0xD5, 0xF9 };
    }
}
