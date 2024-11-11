using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace Common.DataTransfer
{
    public class ByteSerializer<T> where T : struct
    {

        public static T Deserialize(byte[] bytes)
        {
            return MemoryMarshal.Cast<byte, T>(bytes)[0];
        }

        public static byte[] Serialize(T data)
        {
            return Encoding.ASCII.GetBytes(data.ToString());
        }
    }
}
