namespace SerialPortLibrary
{
    /// <summary>
    /// Lớp thư viện chứa các phương thức xử lý với cổng kết nối
    /// </summary>
    public class Sensor : ISerialPortLb
    {
        readonly SerialPortLb _port;
        public Sensor(ISerialPortLb iSerialPortLb ) 
        {
            _port = (SerialPortLb?)iSerialPortLb;
        }
        public Action<string> Message { get; set; }
        public event EventHandler<PhChangedHandlerEventArgs> PhChanged;
        public event EventHandler<TempChangedHandlerEventArgs> TempChanged;
        public async Task<int> ReadAsync()
        {
            return await SerialPortLb.ReadAsync(() =>
            {
                List<string> a = new();
                for (int i = 0; i < 7; i++)
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
                        }
                        if (a[0] == "2")
                        {
                            var rs = (float)result / 10;
                            Message?.Invoke($"Nhiệt độ:{rs}");
                            OnTemChanged(rs);
                        }
                    }
                }return 0;
            });
        }
        //public async Task WriteAsync(byte[] buffer) 
        //{
        //    await SerialPortLb.WriteAsync((byte[] buffer) =>
        //    {
        //        _port.Write(buffer, 0, buffer.Length);           
        //    });
        //}
        /// <summary>
        /// Ghi/Gửi dữ liệu và lệnh vào cổng kết nối 
        /// </summary>
        /// <returns></returns>
        public async Task WriteAsync(byte[] buffer)
        {
            Task Write = new(() =>
            {
                _port.Write(buffer, 0, buffer.Length);
            }
            );
            Write.Start();
            await Write;
        }
        /// <summary>
        /// Đọc dữ liệu từ cổng kết nối và xử lý đưa ra kết quả tương ứng
        /// </summary>
        /// <returns></returns>
        //public async Task ReadAsync()
        //{
        //    Task Read = new(() =>
        //    {
        //        List<string> a = new();
        //        for(int i = 0; i < 7; i++)
        //        {
        //            var m = _port.ReadChar().ToString("X");
        //            a.Add(m);
        //            if (a.Count == 7)
        //            {
        //                int result = int.Parse(a[3] + a[4], System.Globalization.NumberStyles.HexNumber);
        //                if (a[0] == "1")
        //                {
        //                    Message?.Invoke($"PH:{result}");
        //                    OnPhChanged(result);
        //                }
        //                if (a[0] == "2")
        //                {
        //                    var rs = (float)result / 10;
        //                    Message?.Invoke($"Nhiệt độ:{rs}");
        //                    OnTemChanged(rs);
        //                }
        //            }
        //        }
        //    }
        //    );
        //    Read.Start();
        //    await Read;
        //}
        /// <summary>
        /// Khởi chạy các phương thức tổng thể trong kết nối
        /// </summary>
        public void Run(byte[] a)
        {
            _port.Open();
            Task.WaitAll(WriteAsync(a), ReadAsync());
            _port.Close();
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
   

}
