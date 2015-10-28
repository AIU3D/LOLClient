namespace NetFrame.Auto
{
    /// <summary>
    /// 消息体的编码与解码,写入数据和读取数据必须一致
    /// </summary>
    public class MessageEncoding
    {
        /// <summary>
        /// 消息体序列化
        /// </summary>
        /// <param name="value">消息体</param>
        /// <returns></returns>
        public static byte[] MessageEncode(object value)
        {
            SocketModel model = value as SocketModel;
            ByteArray ba = new ByteArray();
            ba.Write(model.Type);
            ba.Write(model.Area);
            ba.Write(model.Command);

            if (model.Message != null)
            {
                ba.Write(SerializeUtil.Encode(model.Message));                
            }

            byte[] result = ba.GetBuff();
            ba.Close();
            return result;
        }

        /// <summary>
        /// 消息体的反序列化
        /// </summary>
        /// <param name="value">序列化的二进制码</param>
        /// <returns></returns>
        public static object MessageDecode(byte[] value)
        {
            ByteArray ba = new ByteArray(value);
            SocketModel model = new SocketModel();
            byte type;
            int area;
            int command;
            ba.Read(out type);
            ba.Read(out area);
            ba.Read(out command);
            model.Type = type;
            model.Area = area;
            model.Command = command;

            if (ba.ReadEnable)
            {
                byte[] message;
                ba.Read(out message, ba.Length - ba.Position);
                model.Message = SerializeUtil.Decode(message);
            }
            ba.Close();
            return model;

        }
    }
}