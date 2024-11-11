using System.Text;

namespace Common.DataTransfer
{
    public static class ArrayByteSerializer<T> where T : struct
    {
        public static T[] Deserialize(byte[] bytes)
        {
            int structSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            int elementCount = bytes.Length / structSize;

            T[] deserializedArray = new T[elementCount];

            Buffer.BlockCopy(bytes, 0, deserializedArray, 0, bytes.Length);

            return deserializedArray;
        }

        public static byte[] Serialize(T[] data)
        {
            List<byte> result = new List<byte>(1024);
            for (int i = 0; i < data.Length; i++) 
            {
                byte[] binarySerializedElement = Encoding.ASCII.GetBytes(data[i].ToString());
                result.AddRange(binarySerializedElement);
            }
            return result.ToArray();
        }
    }
}
