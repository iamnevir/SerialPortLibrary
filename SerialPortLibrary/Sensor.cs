using Newtonsoft.Json;
using System.Collections;
using System.IO.Ports;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace SerialPortLibrary
{
    /// <summary>
    /// Lớp thư viện chứa các phương thức xử lý với cổng kết nối
    /// </summary>
    [InitializationDescription("Khi khởi tạo object cần đưa vào tham số ReadTimeOut và WriteTimeOut")]
    public class Sensor
    {
        bool _continue = true;
        SerialPort _port;
        /// <summary>
        /// Constructor chứa tham số truyền vào
        /// </summary>
        /// <param name="serialConfig">object của lớp SerialConfig</param>
        /// <param name="readTimeOut">Đặt Số mili giây trước khi hết thời gian chờ nếu thao tác đọc không kết thúc</param>
        /// <param name="writeTimeOut">Đặt Số mili giây trước khi hết thời gian chờ nếu thao tác ghi không kết thúc</param>
        public Sensor(SerialConfig serialConfig, int readTimeOut, int writeTimeOut)
        {
            _port = new (serialConfig.PortName, serialConfig.BaudRate)
            {
                ReadTimeout = readTimeOut,
                WriteTimeout = writeTimeOut
            };
        }
        public Action<string> Log { get; set; }
        public Action<string> Message { get; set; }
        public event EventHandler<PhChangedHandlerEventArgs> PhChanged;
        public event EventHandler<TempChangedHandlerEventArgs> TempChanged;
        /// <summary>
        /// Phương thức mở cổng kết nối mới, cho biết sẵn sàng kết nối
        /// </summary>
        public void Open()
        {
            _port.Open();
        }
        /// <summary>
        /// Đóng kết nối cổng, đặt thuộc tính IsOpen thành false và xử lý đối tượng Luồng bên trong
        /// </summary>
        public void Close()
        {
            if (_port.IsOpen)
              _port.Close();
        }
        /// <summary>
        /// Ghi/Gửi dữ liệu và lệnh vào cổng kết nối mỗi 500 mili giây
        /// </summary>
        /// <returns></returns>
        public async Task Write()
        {
            Task Write = new(() =>
            {
                while (_continue)
                {
                    try
                    {
                        _port.Write(DataSend.PhData, 0, DataSend.PhData.Length);
                        Thread.Sleep(500);
                        _port.Write(DataSend.Data, 0, DataSend.Data.Length);
                        Thread.Sleep(500);
                    }
                    catch (Exception)
                    {
                        _continue = false;
                        _port.ErrorReceived += Port_ErrorReceived;
                    }
                }
            }
            );
            Write.Start();
            await Write;
        }
        /// <summary>
        /// Xử lý sự kiện lỗi đường truyền kết nối
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Log?.Invoke("Error line Unknown!");
        }
        /// <summary>
        /// Đọc dữ liệu từ cổng kết nối và xử lý đưa ra kết quả tương ứng
        /// </summary>
        /// <returns></returns>
        public async Task Read()
        {
            Task Read = new(() =>
            {
                List<string> a = new();
                while (_continue)
                {
                    try
                    {

                        var m = _port.ReadChar().ToString("X");
                        a.Add(m);
                        if (a.Count == 7)
                        {
                            int result = int.Parse(a[3] + a[4], System.Globalization.NumberStyles.HexNumber);
                            if (a[0] == "1")
                            {
                                Message?.Invoke($"PH:{result}");
                                OnPhChanged(result);
                                SaveData(result,"PH:");
                            }
                            if (a[0] == "2")
                            {
                                var rs = (float)result / 10;
                                Message?.Invoke($"Nhiệt độ:{rs}");
                                OnTemChanged(rs);
                                SaveData(rs, "Nhiệt độ:");
                            }
                            
                            a = new List<string>();

                        }
                    }
                    catch (TimeoutException) { }
                }
            }
            );
           
            Read.Start();
            await Read;
        }
        /// <summary>
        /// Lưu dữ liệu đọc được vào file json
        /// </summary>
        /// <param name="result"></param>
        /// <param name="ms"></param>
        public static void SaveData(object result,string ms)
        {
            using var stream = File.OpenWrite("Sensor.json");
            JsonSerializer serializer = new();           
            using StreamWriter sWriter = new(stream);
            using JsonWriter jWriter = new JsonTextWriter(sWriter);
            serializer.Serialize(jWriter,ms+result);
        }
        /// <summary>
        /// Đọc dữ liệu đã ghi được trong quá trình kết nối
        /// </summary>
        public static void Load()
        {
            using var stream = File.OpenRead("Sensor.json");
            JsonSerializer serializer = new();
            using StreamReader sReader = new(stream);
            using JsonReader jReader = new JsonTextReader(sReader);
            var result = serializer.Deserialize(jReader);
        }
        /// <summary>
        /// Khởi chạy các phương thức tổng thể trong kết nối
        /// </summary>
        public async Task Run()
        {
            Open();
            Task.WaitAll( Write(),Read());
            Close();
            await Run();
        }
        /// <summary>
        /// Giải phóng tài nguyên và reset lại cổng kết nối
        /// </summary>
        public void Reset()
        {
            _port.Dispose();
            _port.Disposed += Port_Disposed;
            _port.Close();
            _port.Open();
        }

        private void Port_Disposed(object? sender, EventArgs e)
        {
            Log?.Invoke("Tài nguyên đã được giải phóng!");
        }

        private void OnPhChanged(int ph)
        {
            PhChanged?.Invoke(this, new PhChangedHandlerEventArgs(ph));
        }
        private void OnTemChanged(float tem)
        {
            TempChanged?.Invoke(this, new TempChangedHandlerEventArgs(tem));
        }
    }
    /// <summary>
    /// lớp xử lý sự kiện lỗi cổng
    /// </summary>
    public class SensorErrorEventArgs : EventArgs
    {
        public string Message { get; set; }
        public SensorErrorEventArgs(string message)
        {
            Message = message;
        }
    }
    /// <summary>
    /// lớp xử lý sự kiện thay đổi độ PH
    /// </summary>
    public class PhChangedHandlerEventArgs : EventArgs
    {
        public int PH { get; set; }
        public PhChangedHandlerEventArgs(int ph)
        {
            PH = ph;
        }
    }
    /// <summary>
    /// lớp xử lý sự kiện thay đối nhiệt độ
    /// </summary>
    public class TempChangedHandlerEventArgs : EventArgs
    {
        public float Tem { get; set; }
        public TempChangedHandlerEventArgs(float tem)
        {
            Tem = tem;
        }
    }
    /// <summary>
    /// Mô tả lưu ý của thư viện
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InitializationDescription : Attribute
    {
        public string Description { get; set; }
        public InitializationDescription(string description)
        {
            Description = description;
        }
    }

}
