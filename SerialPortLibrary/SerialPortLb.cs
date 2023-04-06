using System.IO.Ports;
namespace SerialPortLibrary;
/// <summary>
/// Class này là một wrapper được xây dựng từ System.IO.Ports.SerialPort
/// </summary>
public class SerialPortLb:ISerialPortLb
{
    SerialPort _port;
    bool _isOpen;
    /// <summary>
    /// Hàm khởi tạo cho lớp SerialPortLb
    /// </summary>
    /// <param name="serialConfig">object của lớp SerialConfig dùng để cấu hình cho SerialPortLb</param>
    public SerialPortLb(SerialConfig serialConfig)
    {
        _port = new (serialConfig.PortName,
                     serialConfig.BaudRate,
                     serialConfig.Parity,
                     serialConfig.DataBits,
                     serialConfig.StopBits)
        {
            ReadTimeout = serialConfig.ReadTimeOut,
            WriteTimeout = serialConfig.WriteTimeOut
        };                        
    }
    /// <summary>
    /// Biến kiểm tra trạng thái mở cổng Serial
    /// </summary>
    public bool IsOpen => _isOpen;
    /// <summary>
    /// Sự kiện cho biết rằng một sự kiện tín hiệu phi dữ liệu đã xảy ra trên cổng được đại diện bởi đối tượng SerialPort.
    /// </summary>
    public event EventHandler<SerialPinChangedEventArgs> PinChanged;
    /// <summary>
    /// Sự kiện cho biết rằng dữ liệu đã được nhận thông qua một cổng được đại diện bởi đối tượng SerialPort.
    /// </summary>
    public event EventHandler<SerialDataReceivedEventArgs> DataReceived;
    /// <summary>
    /// Sự kiện cho biết đã xảy ra lỗi với một cổng được đại diện bởi đối tượng SerialPort.
    /// </summary>
    public event EventHandler<SerialErrorReceivedEventArgs> ErrorReceived;
    /// <summary>
    /// Mở một kết nối cổng nối tiếp mới, cho biết cổng đã sẵn sàng cho việc trao đổi dữ liệu.
    /// </summary>
    /// <exception cref="InvalidOperationException">Ngoại lệ được đưa ra khi lệnh gọi phương thức không hợp lệ đối với trạng thái hiện tại của đối tượng.</exception>
    public void Open()
    {
        if (_isOpen)
            throw new InvalidOperationException("Cổng đang được mở!");
        try
        {
            _port.Open();
            _isOpen = true;
        }
        catch (UnauthorizedAccessException)
        {
            throw new ("Truy cập bị từ chối vào cổng.-hoặc-Quy trình hiện tại hoặc quy trình khác trên hệ thống đã có cổng COM được chỉ định mở bằng phiên bản SerialPort hoặc bằng mã không được quản lý.");
        }
        catch (IOException)
        {
            throw new ("Cổng ở trạng thái không hợp lệ.-hoặc-Nỗ lực đặt trạng thái của cổng bên dưới không thành công. Ví dụ: các tham số được truyền từ đối tượng SerialPort này không hợp lệ.");
        }
        catch (ArgumentException)
        {
            throw new ("Tên cổng không bắt đầu bằng COM.-hoặc-Loại tệp của cổng không được hỗ trợ.");
        }
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
    /// Hàm này giải phóng tất cả tài nguyên được hay không được quản lý bởi SerialPort.
    /// hủy đăng ký tất cả các sự kiện.
    /// </summary>
    public void Dispose()
    {
        if (!_isOpen) { return; }
        _port.DataReceived -= null;
        _port.PinChanged -= null;
        _port.ErrorReceived -= null;
        _port.Dispose();
        if (null != DataReceived)
        {
            foreach (var item in DataReceived.GetInvocationList())
            {
                DataReceived -= item as EventHandler<SerialDataReceivedEventArgs>;
            }
        }
        if (null != PinChanged)
        {
            foreach (var item in PinChanged.GetInvocationList())
            {
                PinChanged -= item as EventHandler<SerialPinChangedEventArgs>;
            }
        }

        if (null != ErrorReceived)
        {
            foreach (var item in ErrorReceived.GetInvocationList())
            {
                ErrorReceived -= item as EventHandler<SerialErrorReceivedEventArgs>;
            }
        }
    }
    /// <summary>
    /// Thực hiện đóng cổng 
    /// </summary>
    public void Close()
    {
        CheckOpen();
        try
        {
            _port.Close();
            _isOpen = false;
        }
        catch (IOException) { throw new("Cổng ở trạng thái không hợp lệ."); }
    }
    /// <summary>
    /// Đọc đồng bộ một byte từ bộ đệm đầu vào SerialPort.
    /// </summary>
    /// <returns></returns>
    public int ReadByte()
    {
        CheckOpen();
        try { return _port.ReadByte(); }
        catch (Exception) { return 0; }
    }
    /// <summary>
    /// Đồng bộ đọc một ký tự từ bộ đệm đầu vào SerialPort .
    /// </summary>
    /// <returns></returns>
    public int ReadChar()
    {
        CheckOpen();
        try { return _port.ReadChar(); }
        catch(Exception) { return 0; }
    }
    /// <summary>
    /// Đọc tất cả các byte có sẵn ngay lập tức, dựa trên mã hóa, trong cả luồng và bộ đệm đầu vào của đối tượng SerialPort.
    /// </summary>
    /// <returns></returns>
    public string ReadExisting()
    {
        CheckOpen();
        try { return _port.ReadExisting(); }
        catch (Exception) { return ""; }
    }
    /// <summary>
    /// Đọc một số byte từ bộ đệm đầu vào SerialPort và ghi các byte đó vào một mảng byte ở độ lệch đã chỉ định.
    /// </summary>
    /// <param name="buffer">Mảng byte để ghi đầu vào.</param>
    /// <param name="offset">Phần bù để ghi byte.</param>
    /// <param name="count">Số byte tối đa để đọc. Ít byte được đọc hơn nếu count lớn hơn số byte trong bộ đệm đầu vào.</param>
    /// <returns>Số byte đã đọc.</returns>
    /// <exception cref="ArgumentNullException">Bộ đệm được thông qua là null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Các tham số offset hoặc count nằm ngoài vùng hợp lệ của thông buffer số được truyền. Hoặc offsethoặc countnhỏ hơn 0.</exception>
    /// <exception cref="ArgumentException">offset cộng count lớn hơn độ dài của buffer.</exception>
    /// <exception cref="TimeoutException">Không có byte nào có sẵn để đọc.</exception>
    public int Read(byte[] buffer, int offset, int count)
    {
        CheckOpen();
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));
        if (offset < 0 || count < 0)
            throw new ArgumentOutOfRangeException("biến offset hoặc count nằm ngoài vùng hợp lệ. Hoặc nhỏ hơn 0.");
        if (buffer.Length - offset < count)
            throw new ArgumentException("Kích thước phần bù trong bộ đệm + số byte đọc tối đa lớn hơn kích thước bộ đệm");
        try { return _port.Read(buffer, offset, count); }
        catch (TimeoutException) { throw new TimeoutException("Không có byte nào có sẵn để đọc."); }
    }
    /// <summary>
    /// Đọc một số ký tự từ bộ đệm đầu vào SerialPort và ghi các byte đó vào một mảng byte ở độ lệch đã chỉ định.
    /// </summary>
    /// <param name="buffer">Mảng byte để ghi đầu vào.</param>
    /// <param name="offset">Phần bù để ghi byte.</param>
    /// <param name="count">Số byte tối đa để đọc. Ít byte được đọc hơn nếu count lớn hơn số byte trong bộ đệm đầu vào.</param>
    /// <returns>Số byte đã đọc.</returns>
    /// <exception cref="ArgumentNullException">Bộ đệm được thông qua là null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Các tham số offset hoặc count nằm ngoài vùng hợp lệ của thông buffer số được truyền. Hoặc offsethoặc countnhỏ hơn 0.</exception>
    /// <exception cref="ArgumentException">offset cộng count lớn hơn độ dài của buffer.</exception>
    /// <exception cref="TimeoutException">Không có byte nào có sẵn để đọc.</exception>
    public int Read(char[] buffer, int offset, int count)
    {
        CheckOpen();
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));
        if (offset < 0 || count < 0)
            throw new ArgumentOutOfRangeException((nameof(offset) + nameof(count)), "biến offset hoặc count nằm ngoài vùng hợp lệ. Hoặc nhỏ hơn 0.");
        if (buffer.Length - offset < count || count == 1)
            throw new ArgumentException("Kích thước phần bù trong bộ đệm + số byte đọc tối đa lớn hơn kích thước bộ đệm");
        try { return _port.Read(buffer, offset, count); }
        catch (TimeoutException) { throw new TimeoutException("Không có byte nào có sẵn để đọc."); }
    }
    /// <summary>
    /// Ghi một số byte được chỉ định vào cổng nối tiếp bằng cách sử dụng dữ liệu từ bộ đệm.
    /// </summary>
    /// <param name="buffer">Mảng byte để ghi đầu vào.</param>
    /// <param name="offset">Phần bù để ghi byte.</param>
    /// <param name="count">Số byte tối đa để đọc. Ít byte được đọc hơn nếu count lớn hơn số byte trong bộ đệm đầu vào.</param>
    /// <returns>Số byte đã đọc.</returns>
    /// <exception cref="ArgumentNullException">Bộ đệm được thông qua là null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Các tham số offset hoặc count nằm ngoài vùng hợp lệ của thông buffer số được truyền. Hoặc offsethoặc countnhỏ hơn 0.</exception>
    /// <exception cref="ArgumentException">offset cộng count lớn hơn độ dài của buffer.</exception>
    /// <exception cref="TimeoutException">Không có byte nào có sẵn để đọc.</exception>
    public void Write(byte[] buffer, int offset, int count)
    {
        CheckOpen();
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));
        if (offset < 0 || count < 0)
            throw new ArgumentOutOfRangeException((nameof(offset)+nameof(count)),"biến offset hoặc count nằm ngoài vùng hợp lệ. Hoặc nhỏ hơn 0.");
        if (buffer.Length - offset < count || count == 1)
            throw new ArgumentException("Kích thước phần bù trong bộ đệm + số byte đọc tối đa lớn hơn kích thước bộ đệm");
        try { _port.Write(buffer, offset, count); }
        catch (TimeoutException) { throw new TimeoutException("Không có byte nào có sẵn để đọc."); }
    }
    /// <summary>
    /// Ghi một số ký tự được chỉ định vào cổng nối tiếp bằng cách sử dụng dữ liệu từ bộ đệm.
    /// </summary>
    /// <param name="buffer">Mảng byte để ghi đầu vào.</param>
    /// <param name="offset">Phần bù để ghi byte.</param>
    /// <param name="count">Số byte tối đa để đọc. Ít byte được đọc hơn nếu count lớn hơn số byte trong bộ đệm đầu vào.</param>
    /// <returns>Số byte đã đọc.</returns>
    /// <exception cref="ArgumentNullException">Bộ đệm được thông qua là null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Các tham số offset hoặc count nằm ngoài vùng hợp lệ của thông buffer số được truyền. Hoặc offsethoặc countnhỏ hơn 0.</exception>
    /// <exception cref="ArgumentException">offset cộng count lớn hơn độ dài của buffer.</exception>
    /// <exception cref="TimeoutException">Không có byte nào có sẵn để đọc.</exception>
    public void Write(char[] buffer, int offset, int count)
    {
        CheckOpen();
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));
        if (offset < 0 || count < 0)
            throw new ArgumentOutOfRangeException((nameof(offset) + nameof(count)),"biến offset hoặc count nằm ngoài vùng hợp lệ. Hoặc nhỏ hơn 0.");
        if (buffer.Length - offset < count || count == 1)
            throw new ArgumentException("Kích thước phần bù trong bộ đệm + số byte đọc tối đa lớn hơn kích thước bộ đệm");
        try { _port.Write(buffer, offset, count); }
        catch (TimeoutException) { throw new TimeoutException("Không có byte nào có sẵn để đọc."); }
    }
    /// <summary>
    /// Generic bất đồng bộ cho các phương thức ghi dữ liệu, truyền vào một delegate kiểu Action với một tham số truyền vào và không trả về kết quả.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="write"></param>
    /// <returns></returns>
    public static async Task WriteAsync<T>(Action<T> write)
    {
        await WriteAsync(write);
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
    //public static async Task<T> ReadAsync<T>(Func<T,T> read)
    //{
    //    return await Task.Run(read);
    //}
}
    
/// <summary>
/// interface giúp tạo sự phụ thuộc lỏng cho các lớp thực thi phương thức từ SerialPortLb
/// </summary>
   public interface ISerialPortLb {}
/// <summary>
/// EventArgs for PinChanged
/// </summary>
public class SerialPinChangedEventArgs : EventArgs
{
    /// <summary>
    /// Hàm khởi tạo
    /// </summary>
    /// <param name="eventType"></param>
    public SerialPinChangedEventArgs(SerialPinChange eventType) => EventType = eventType;

    /// <summary>
    /// kiểu event của PinChanged theo enum SerialPinChange của System.IO.Port.
    /// </summary>
    public SerialPinChange EventType { get; }
}

/// <summary>
/// EventArgs for ErrorReceived.
/// </summary>
public class SerialErrorReceivedEventArgs : EventArgs
{
    /// <summary>
    /// Hàm khởi tạo
    /// </summary>
    /// <param name="eventType"></param>
    public SerialErrorReceivedEventArgs(SerialError eventType) => EventType = eventType;

    /// <summary>
    /// Kiểu event của ErrorReceived theo enum SerialError của System.IO.Port.
    /// </summary>
    public SerialError EventType { get; }
}

/// <summary>
/// EventArgs for DataReceived.
/// </summary>
public class SerialDataReceivedEventArgs : EventArgs
{
    /// <summary>
    /// Hàm khởi tạo
    /// </summary>
    /// <param name="eventType"></param>
    public SerialDataReceivedEventArgs(SerialData eventType) => EventType = eventType;

    /// <summary>
    /// Kiểu event của ErrorReceived theo enum SerialData của System.IO.Port.
    /// </summary>
    public SerialData EventType { get; }
}