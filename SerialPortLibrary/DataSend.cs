namespace SerialPortLibrary;

/// <summary>
/// Lớp chứa dữ liệu về gói tin gửi đến cổng kết nối
/// </summary>
public class DataSend
{
    public DataSend(byte[] Data) => _data = Data;
    //private byte[] _phData = { 0x01, 0x03, 0x00, 0x00, 0x00, 0x01, 0x84, 0x0A };
    private byte[] _data = { 0x02, 0x03, 0x00, 0x01, 0x00, 0x01, 0xD5, 0xF9 };
    public byte[] Data
    {
        get { return _data; }
        set { _data = value; }
    }
}
