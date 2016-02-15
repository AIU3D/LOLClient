using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace NetFrame.Auto
{
    /// <summary>
    /// 长度解码
    /// </summary>
    public class LengthEncoding
    {
        /// <summary>
        /// 长度编码
        /// </summary>
        /// <param name="buff">消息体</param>
        /// <returns>返回编码后的结果</returns>
        public static byte[] Encode(byte[] buff)
        {
            MemoryStream ms = new MemoryStream();  //创建内存流对象
            BinaryWriter bw = new BinaryWriter(ms);//写入二进制对象流
            //写入消息长度
            bw.Write(buff.Length);
            //写入消息体，在消息体前面加入消息长度
            bw.Write(buff);

            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int) ms.Length);
            bw.Close();
            ms.Close();
            return result;
        }

        /// <summary>
        /// 黏包长度解码
        /// </summary>
        /// <param name="cache">传入缓存对象</param>
        /// <returns></returns>
        public static byte[] Decode(ref List<byte> cache)
        {
            if (cache.Count < 4) return null;
            MemoryStream ms = new MemoryStream(cache.ToArray());
            BinaryReader br = new BinaryReader(ms);
            int length = br.ReadInt32();             //读取消息的长度

            //如果消息体长度大于缓存数据中数据长度，说明消息还没有读取完，等待下次消息到达后再次处理
            if (length > ms.Length - ms.Position)
            {
                return null;
            }

            //读取正确长度的数据
            byte[] result = br.ReadBytes(length);
            //清空缓存
            cache.Clear();
            //将读取后的剩余数据写入缓存
            cache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
            br.Close();
            ms.Close();

            return result;
        }
    }
}