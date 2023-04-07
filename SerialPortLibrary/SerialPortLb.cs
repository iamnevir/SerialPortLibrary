using System.IO.Ports;
namespace SerialPortLibrary;
/// <summary>
/// Class này là một wrapper được xây dựng từ System.IO.Ports.SerialPort
/// </summary>
public class SerialPortLb : ISerialPortLb
{
    SerialPort _port;
    bool _isOpen;
    /// <summary>
    /// Hàm khởi tạo cho lớp SerialPortLb
    /// </summary>
    /// <param name="serialConfig">object của lớp SerialConfig dùng để cấu hình cho SerialPortLb</param>
    public SerialPortLb(SerialConfig serialConfig)
    {
        _port = new()
        {
            PortName = serialConfig.PortName,
            BaudRate = serialConfig.BaudRate,
            Parity = serialConfig.Parity,
            DataBits = serialConfig.DataBits,
            StopBits = serialConfig.StopBits,
            ReadTimeout = serialConfig.ReadTimeOut,
            WriteTimeout = serialConfig.WriteTimeOut
        };
    }

    public SerialPort Port { get => _port; set => _port = value; }

    /// <summary>
    /// Mở một kết nối cổng nối tiếp mới, cho biết cổng đã sẵn sàng cho việc trao đổi dữ liệu.
    /// </summary>
    /// <exception cref="InvalidOperationException">Ngoại lệ được đưa ra khi lệnh gọi phương thức không hợp lệ đối với trạng thái hiện tại của đối tượng.</exception>
    public void Open()
    {
        if (!_isOpen)
            return;
        Port.Open();
        _isOpen = true;
    }

    /// <summary>
    /// Hàm kiểm tra trạng thái cổng hiện tại.
    /// Hàm này sẽ được sử dụng trong các phương thực thực thi dữ liệu để đảm bảo tính kết nối trước khi thực thi.
    /// </summary>
    /// <exception cref="InvalidOperationException">Ngoại lệ được đưa ra khi lệnh gọi phương thức không hợp lệ đối với trạng thái hiện tại của đối tượng.</exception>
    void CheckOpen()
    {
        if (!_isOpen)
            throw new InvalidOperationException("Cổng được chỉ định không mở.");
    }

    /// <summary>
    /// Thực hiện đóng cổng 
    /// </summary>
    public void Close()
    {
        CheckOpen();
        Port.Close();
        _isOpen = false;
    }

    /// <summary>
    /// Đọc đồng bộ một byte từ bộ đệm đầu vào SerialPort.
    /// </summary>
    /// <returns></returns>
    public int ReadByte()
    {
        CheckOpen();
        return Port.ReadByte();
    }

    /// <summary>
    /// Đồng bộ đọc một ký tự từ bộ đệm đầu vào SerialPort .
    /// </summary>
    /// <returns></returns>
    public int ReadChar()
    {
        CheckOpen();
        return Port.ReadChar();
    }

    /// <summary>
    /// Đọc tất cả các byte có sẵn ngay lập tức, dựa trên mã hóa, trong cả luồng và bộ đệm đầu vào của đối tượng SerialPort.
    /// </summary>
    /// <returns></returns>
    public string ReadExisting()
    {
        CheckOpen();
        return Port.ReadExisting();
    }

    /// <summary>
    /// Đọc một số byte từ bộ đệm đầu vào SerialPort và ghi các byte đó vào một mảng byte ở độ lệch đã chỉ định.
    /// </summary>
    /// <param name="buffer">Mảng byte để ghi đầu vào.</param>
    /// <param name="offset">Phần bù để ghi byte.</param>
    /// <param name="count">Số byte tối đa để đọc. Ít byte được đọc hơn nếu count lớn hơn số byte trong bộ đệm đầu vào.</param>
    /// <returns>Một giá trị int là số byte đã đọc được.</returns>
    public int Read(byte[] buffer, int offset, int count)
    {
        CheckOpen();
        return Port.Read(buffer, offset, count);
    }

    /// <summary>
    /// Đọc một số ký tự từ bộ đệm đầu vào SerialPort và ghi các byte đó vào một mảng byte ở độ lệch đã chỉ định.
    /// </summary>
    /// <param name="buffer">Mảng byte để ghi đầu vào.</param>
    /// <param name="offset">Phần bù để ghi byte.</param>
    /// <param name="count">Số byte tối đa để đọc. Ít byte được đọc hơn nếu count lớn hơn số byte trong bộ đệm đầu vào.</param>
    /// <returns>Số byte đã đọc.</returns>
    public int Read(char[] buffer, int offset, int count)
    {
        CheckOpen();
        return Port.Read(buffer, offset, count);
    }

    /// <summary>
    /// Ghi một số byte được chỉ định vào cổng nối tiếp bằng cách sử dụng dữ liệu từ bộ đệm.
    /// </summary>
    /// <param name="buffer">Mảng byte để ghi đầu vào.</param>
    /// <param name="offset">Phần bù để ghi byte.</param>
    /// <param name="count">Số byte tối đa để đọc. Ít byte được đọc hơn nếu count lớn hơn số byte trong bộ đệm đầu vào.</param>
    /// <returns>Số byte đã đọc.</returns>
    public void Write(byte[] buffer, int offset, int count)
    {
        CheckOpen();
        Port.Write(buffer, offset, count);
    }

    /// <summary>
    /// Ghi một số ký tự được chỉ định vào cổng nối tiếp bằng cách sử dụng dữ liệu từ bộ đệm.
    /// </summary>
    /// <param name="buffer">Mảng byte để ghi đầu vào.</param>
    /// <param name="offset">Phần bù để ghi byte.</param>
    /// <param name="count">Số byte tối đa để đọc. Ít byte được đọc hơn nếu count lớn hơn số byte trong bộ đệm đầu vào.</param>
    /// <returns>Số byte đã đọc.</returns>
    public void Write(char[] buffer, int offset, int count)
    {
        CheckOpen();
        Port.Write(buffer, offset, count);
    }
    /// <summary>
    /// Generic bất đồng bộ cho các phương thức ghi dữ liệu, truyền vào một delegate kiểu Action với một tham số truyền vào và không trả về kết quả.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="write"></param>
    /// <returns></returns>
    public static async Task WriteAsync(Action write)
    {
        await Task.Run(write);
    }
    /// <summary>
    /// Generic bất đồng bộ cho các phương thức đọc dữ liệu, truyền vào một delegate kiểu Action với một tham số truyền vào và không trả về kết quả.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="read"></param>
    /// <returns></returns>
    public static async Task<T> ReadAsync<T>(Func<T> read)
    {
        return await Task.Run(read);
    }
}

/// <summary>
/// interface giúp tạo sự phụ thuộc lỏng cho các lớp thực thi phương thức từ SerialPortLb
/// </summary>
public interface ISerialPortLb { }
