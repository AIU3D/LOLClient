using System.Collections.Generic;
using System.Net.Sockets;

namespace NetFrame
{
    public delegate byte[] LengthEncode(byte[] value);

    public delegate byte[] LengthDecode(ref List<byte> value);

    public delegate byte[] MessageEncode(object value);

    public delegate object MessageDecode(byte[] value);

    public delegate void SendProcess(SocketAsyncEventArgs e);

    public delegate void ColseProcess(UserToken token, string error);

}