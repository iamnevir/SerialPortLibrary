using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace SerialPortLibrary
{
    /// <summary>
    /// Lớp thư viện chứa các phương thức xử lý với cổng kết nối
    /// </summary>
    [InitializationDescription("Khi khởi tạo object cần đưa vào tham số ReadTimeOut và WriteTimeOut")]
    public class Senior
    {
        public event EventHandler<PhChangedHandler> PhChanged;
        public event EventHandler<TempChangedHandler> TempChanged;
        bool _continue = true;
        private SerialPort port;
        /// <summary>
        /// Constructor chứa tham số truyền vào
        /// </summary>
        /// <param name="serialConfig">object của lớp SerialConfig</param>
        /// <param name="readTimeOut">Đặt Số mili giây trước khi hết thời gian chờ nếu thao tác đọc không kết thúc</param>
        /// <param name="writeTimeOut">Đặt Số mili giây trước khi hết thời gian chờ nếu thao tác ghi không kết thúc</param>
        public Senior(SerialConfig serialConfig, int readTimeOut, int writeTimeOut)
        {
            port = new SerialPort(serialConfig.PortName, serialConfig.BaudRate);
            port.ReadTimeout = readTimeOut;
            port.WriteTimeout = writeTimeOut;
        }
        /// <summary>
        /// Phương thức mở cổng kết nối mới, cho biết sẵn sàng kết nối
        /// </summary>
        public void Open()
        {
            if (port.IsOpen) port.Close();
            port.Open();
        }
        /// <summary>
        /// Đóng kết nối cổng, đặt thuộc tính IsOpen thành false và xử lý đối tượng Luồng bên trong
        /// </summary>
        public void Close()
        {
            port.Close();
        }
        /// <summary>
        /// Ghi/Gửi dữ liệu và lệnh vào cổng kết nối mỗi 500 mili giây
        /// </summary>
        /// <returns></returns>
        public async Task Write()
        {
            while (_continue)
            {
                try
                {
                    port.Write(DataSend.PhData, 0, DataSend.PhData.Length);
                    port.Write(DataSend.data, 0, DataSend.data.Length);
                }
                catch (Exception ex)
                {
                    _continue = false;
                    port.ErrorReceived += Port_ErrorReceived;
                }
            }
            Write().Start();
            await Task.Delay(500);
        }
        /// <summary>
        /// Xử lý sự kiện lỗi đường truyền kết nối
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("Error line Unknown!");
        }
        /// <summary>
        /// Đọc dữ liệu từ cổng kết nối và xử lý đưa ra kết quả tương ứng
        /// </summary>
        /// <returns></returns>
        public async Task Read()
        {

            List<string> a = new List<string>();
            while (_continue)
            {
                try
                {

                    var m = port.ReadChar().ToString("X");
                    a.Add(m);
                    if (a.Count == 7)
                    {
                        int result = int.Parse(a[3] + a[4], System.Globalization.NumberStyles.HexNumber);
                        if (a[0] == "1")
                        {
                            Console.WriteLine($"PH: {result}");
                            OnPhChanged(result);
                        }
                        if (a[0] == "2")
                        {
                            var rs = (float)result / 10;
                            Console.WriteLine($"Nhiệt độ: {(float)result / 10} ");
                            OnTemChanged(rs);
                        }
                        SaveData(result);
                        a = new List<string>();

                    }
                }
                catch (TimeoutException) { }
            }
            Read().Start();
            await Read();
        }
        /// <summary>
        /// Lưu dữ liệu đọc được vào file json
        /// </summary>
        /// <param name="result"></param>
        static void SaveData(int result)
        {
            FileStream _file = File.Create("data.json");
            using (var stream = File.OpenWrite("data.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                using (StreamWriter sWriter = new StreamWriter(_file))
                using (JsonWriter jWriter = new JsonTextWriter(sWriter))
                {
                    serializer.Serialize(jWriter, result);
                }
            }
        }
        /// <summary>
        /// Đọc dữ liệu đã ghi được trong quá trình kết nối
        /// </summary>
        public void Load()
        {
            int result;
            using (var stream = File.OpenRead("data.json"))
            {
                var reader = new StreamReader(stream);
                var serializer = new JsonSerializer();
                result = (int)serializer.Deserialize(reader, typeof(int));
            }
            Console.WriteLine(result.ToString());
        }
        /// <summary>
        /// Khởi chạy các phương thức tổng thể trong kết nối
        /// </summary>
        public void Run()
        {
            Open();
            _ = Write();
            _ = Read();
            Close();
        }
        /// <summary>
        /// Giải phóng tài nguyên và reset lại cổng kết nối
        /// </summary>
        public void Reset()
        {
            port.Dispose();
            port.Disposed += Port_Disposed;
            port.Close();
            port.Open();
        }

        private void Port_Disposed(object? sender, EventArgs e)
        {
            Console.WriteLine("Tài nguyên đã được giải phóng!");
        }

        private void OnPhChanged(int ph)
        {
            if (PhChanged != null)
            {
                PhChanged(this, new PhChangedHandler(ph));
            }
        }
        private void OnTemChanged(float tem)
        {
            if (TempChanged != null)
            {
                TempChanged(this, new TempChangedHandler(tem));
            }
        }
    }
    public class SeniorError : EventArgs
    {
        public string Message { get; set; }
        public SeniorError(string message)
        {
            Message = message;
        }
    }
    public class PhChangedHandler : EventArgs
    {
        public int PH { get; set; }
        public PhChangedHandler(int ph)
        {
            PH = ph;
        }
    }
    public class TempChangedHandler : EventArgs
    {
        public float Tem { get; set; }
        public TempChangedHandler(float tem)
        {
            Tem = tem;
        }
    }


}
