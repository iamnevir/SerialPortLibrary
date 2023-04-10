using System.Text.Json;

namespace SerialPortLibrary;

public sealed class ConfigManager<T> where T : class, new()
{
    readonly string name = $"{typeof(T).ToString().ToLower()}.Json";

    private static readonly ConfigManager<T> _instance = new();

    public static ConfigManager<T> Instance
    {
        get
        {
            if (File.Exists(_instance.name))
                _instance.Load();
            else
                _instance.Create();
            return _instance;
        }
    }

    public T Config { get; set; }

    public void Load()
    {
        Config = new T();
        using var stream = File.OpenRead(name);
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();
        Config = JsonSerializer.Deserialize(json, typeof(T)) as T;
    }

    public void Save()
    {
        Config = new T();
        using var stream = File.OpenWrite(name);
        JsonSerializer.Serialize(stream, Config, typeof(T));
    }

    public void Create()
    {
        Config = new T();
        using FileStream createFile = File.Create(name);
        JsonSerializer.Serialize(createFile, Config, typeof(T));
    }
}