namespace SerialPortLibrary;

/// <summary>
/// Lớp thư viện chứa các phương thức xử lý với cổng kết nối
/// </summary>
public class Sensor : ISerialPortLb
{
    readonly SerialPortLb _port;

    public Sensor(ISerialPortLb iSerialPortLb)
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
                var m = _port.Port.ReadChar().ToString("X");
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
            }
            return 0;
        });
    }

    /// <summary>
    /// Đọc dữ liệu từ cổng kết nối và xử lý đưa ra kết quả tương ứng
    /// </summary>
    /// <returns>Trả về số byte đã đọc</returns>
    public async Task<int> ReadAsync(byte[] buffer)
    {
        return await SerialPortLb.ReadAsync(() =>
        {
            int a = _port.Port.Read(buffer, 0, buffer.Length);
            Message?.Invoke($"Số byte đọc được là: {a}");
            return 0;
        });
    }

    /// <summary>
    /// Ghi/Gửi dữ liệu và lệnh vào cổng kết nối 
    /// </summary>
    public async Task WriteAsync(byte[] buffer)
    {
        await SerialPortLb.WriteAsync(() =>
        {
            _port.Port.Write(buffer, 0, buffer.Length);
        });
    }

    /// <summary>
    /// Khởi chạy các phương thức tổng thể trong kết nối, đây là một phương thức nhanh giúp rút ngắn thời gian kết nối đọc ghi cổng
    /// không nên được sử dụng thường xuyên, chỉ sử dụng trong trường hợp phù hợp
    /// </summary>
    public void Run(byte[] a, byte[] b)
    {
        _port.Port.Open();
        Task.WaitAll(WriteAsync(a), ReadAsync(b));
        _port.Port.Close();

    }

    /// <summary>
    /// Khởi chạy các phương thức tổng thể trong kết nối, đây là một phương thức nhanh giúp rút ngắn thời gian kết nối đọc ghi cổng
    /// không nên được sử dụng thường xuyên, chỉ sử dụng trong trường hợp phù hợp
    /// </summary>
    public void Run(byte[] a)
    {
        _port.Port.Open();
        Task.WaitAll(WriteAsync(a), ReadAsync());
        _port.Port.Close();
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
