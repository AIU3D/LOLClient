using System;
using System.IO;

namespace NetFrame
{
    /// <summary>
    /// 将数据写入成二进制
    /// </summary>
    public class ByteArray
    {
        private MemoryStream ms = new MemoryStream();

        private BinaryWriter bw;
        private BinaryReader br;
       
        /// <summary>
        /// 获取当前数据读取到的下标位置
        /// </summary>
        public int Position { get { return (int) ms.Position; } }

        /// <summary>
        /// 获取当前数据长度
        /// </summary>
        public int Length{get { return (int) ms.Length; }}

        /// <summary>
        /// 当前是否还有数据可以读取
        /// </summary>
        public bool ReadEnable { get { return ms.Length > ms.Position; } }

        public void Close()
        {
            br.Close();
            bw.Close();
            ms.Close();
        }

        public ByteArray()
        {
            bw = new BinaryWriter(ms);
            br = new BinaryReader(ms);
        }
        /// <summary>
        /// 支持传入初始数据的构造
        /// </summary>
        /// <param name="buff">需要写入的数据</param>
        public ByteArray(byte[] buff)
        {
            ms = new MemoryStream(buff);
            bw = new BinaryWriter(ms);
            br = new BinaryReader(ms);
        }

        public void Reposition()
        {
            ms.Position = 0;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuff()
        {
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, (int) ms.Length);
            return result;
        }


        public void Write(int value)
        {
            bw.Write(value);
        }

        public void Write(byte value)
        {
            bw.Write(value);
        }

        public void Write(bool value)
        {
            bw.Write(value);
        }

        public void Write(string value)
        {
            bw.Write(value);
        }

        public void Write(byte[] value)
        {
            bw.Write(value);
        }

        public void Write(double value)
        {
            bw.Write(value);
        }

        public void Write(float value)
        {
            bw.Write(value);
        }

        public void Write(long value)
        {
            bw.Write(value);
        }

        public void Read(out int value)
        {
            value = br.ReadInt32();
        }

        public void Read(out byte value)
        {
            value = br.ReadByte();
        }

        public void Read(out bool value)
        {
            value = br.ReadBoolean();
        }

        public void Read(out string value)
        {
            value = br.ReadString();
        }

        public void Read(out byte[] value,int length)
        {
            value = br.ReadBytes(length);
        }

        public void Read(out double value)
        {
            value = br.ReadDouble();
        }

        public void Read(out float value)
        {
            value = br.ReadSingle();
        }

        public void Read(out long value)
        {
            value = br.ReadInt64();
        }
    }
}