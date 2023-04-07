using System.IO.Ports;

namespace SerialPortLibrary;

/// <summary>
/// Lớp khởi tạo tham số cho cổng kết nối
/// </summary>
public class SerialConfig
{
    private int dataBits = 8;

    public SerialConfig(string portName, int baudRate = 4800, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
    {
        PortName = portName;
        BaudRate = baudRate;
        Parity = parity;
        DataBits = dataBits;
        StopBits = stopBits;
    }

    /// <summary>
    /// Tên cổng truyền vào
    /// </summary>
    public string PortName { get; set; } = "COM2";

    /// <summary>
    /// Tốc độ truyền
    /// </summary>
    public int BaudRate { get; set; } = 4800;

    /// <summary>
    /// Nhận hoặc đặt giao thức kiểm tra chẵn lẻ.
    /// </summary>
    public Parity Parity { get; set; } = Parity.None;

    /// <summary>
    ///  Nhận hoặc đặt số lượng bit dừng tiêu chuẩn trên mỗi byte  
    /// </summary>
    public StopBits StopBits { get; set; } = StopBits.One;

    /// <summary>
    /// Nhận hoặc đặt giao thức bắt tay để truyền dữ liệu qua cổng nối tiếp bằng cách sử dụng một giá trị từ Bắt tay .
    /// </summary>
    public Handshake Handshake { get; set; } = Handshake.None;

    /// <summary>
    /// Nhận hoặc đặt độ dài tiêu chuẩn của bit dữ liệu trên mỗi byte.
    /// </summary>
    public int DataBits
    {
        get => dataBits; set
        {
            if (dataBits >= 5 && dataBits <= 8)
            {
                dataBits = value;
            }

        }
    }
    /// <summary>
    /// Nhận hoặc đặt số mili giây trước khi hết thời gian chờ khi thao tác đọc không kết thúc.
    /// </summary>
    public int ReadTimeOut { get; set; } = 500;

    /// <summary>
    /// Nhận hoặc đặt số mili giây trước khi hết thời gian chờ khi thao tác ghi không kết thúc.
    /// </summary>
    public int WriteTimeOut { get; set; } = 500;


}
