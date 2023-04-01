using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortLibrary
{
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
