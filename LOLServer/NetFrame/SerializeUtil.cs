using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetFrame
{
    /// <summary>
    /// 序列化工具
    /// </summary>
    public class SerializeUtil
    {
        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] Encode(object value)
        {
            MemoryStream ms = new MemoryStream();           //创建编码解码的内存流对象
            BinaryFormatter bf = new BinaryFormatter();     //二进制流序列化对象

            //将obj对象序列化二进制数据写入到内存流
            bf.Serialize(ms,value);
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int) ms.Length);
            ms.Close();
            return result;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Decode(byte[] value)
        {
            MemoryStream ms = new MemoryStream(value);      //创建编码解码的内存流对象,将需要反序列化的数据写入其中
            BinaryFormatter bf = new BinaryFormatter();     //二进制流序列化对象

            //将流数据发序列化为obj对象
            object result = bf.Deserialize(ms);
            ms.Close();
            return result;
        }
    }
}