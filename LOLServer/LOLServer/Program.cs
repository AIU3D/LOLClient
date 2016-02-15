using System;
using NetFrame;
using NetFrame.Auto;

namespace LOLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //服务器初始化
            ServerStart serverStart = new ServerStart(9000);
            serverStart.messageEncode = MessageEncoding.MessageEncode;
            serverStart.messageDecode = MessageEncoding.MessageDecode;
            serverStart.lengthEncode = LengthEncoding.Encode;
            serverStart.lengthDecode = LengthEncoding.Decode;
            serverStart.center = new HandlerCenter();
            serverStart.Start(6650);
            Console.WriteLine("服务器连接成功");
            while (true) { }
        }
    }
}
