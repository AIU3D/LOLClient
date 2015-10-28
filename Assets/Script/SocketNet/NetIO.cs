using System;
using System.Collections.Generic;
using System.Net.Sockets;
using NetFrame;
using NetFrame.Auto;
using UnityEngine;

/// <summary>
/// 联网的IO，为单例模式
/// </summary>
public class NetIO
{
    private static NetIO instance;
    
    private Socket socket;
    private string ip = "127.0.0.1";
    private int port = 6650;

    private byte[] readBuff = new byte[1024];

    private List<byte> cache = new List<byte>();
    private bool isReading = false;

    public List<SocketModel> messages = new List<SocketModel>();   //让unity每一帧去读取信息，如果有的话则处理消息 
    public static NetIO Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NetIO();
            }
            return instance;
        }
    }

    private NetIO()
    {
        try
        {
            //创建客户端连接
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接到服务器
            socket.Connect(ip, port);
            //开启异步接收,消息到达会直接写入缓冲区
            socket.BeginReceive(readBuff, 0, 1024, SocketFlags.None, ReceiveCallBack, readBuff);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }

    /// <summary>
    /// 收到消息回调，结束异步接收
    /// </summary>
    /// <param name="asyncResult"></param>
    private void ReceiveCallBack(IAsyncResult asyncResult)
    {
        try
        {
            //获取当前收到的消息长度
            int length = socket.EndReceive(asyncResult);
            byte[] message = new byte[length];
            Buffer.BlockCopy(readBuff, 0, message, 0, length);
            cache.AddRange(message);

            //与服务器逻辑一样
            if (!isReading)
            {
                isReading = true;
                OnData();
            }

            //消息接收完毕后它会自动关闭，所以进行尾递归，再次开启消息接收
            socket.BeginReceive(readBuff, 0, 1024, SocketFlags.None, ReceiveCallBack, readBuff);
       
        }
        catch (Exception e)
        {
           Debug.Log("远程服务器主动断开连接");
            socket.Close();
        }
    }

    /// <summary>
    /// 将消息体编码
    /// </summary>
    /// <param name="type"></param>
    /// <param name="area"></param>
    /// <param name="command"></param>
    /// <param name="message"></param>
    public void Write(byte type, int area, int command, object message)
    {
        ByteArray ba = new ByteArray();
        ba.Write(type);
        ba.Write(area);
        ba.Write(command);

        if (message != null)
        {
            ba.Write(SerializeUtil.Encode(message));
        }

        //进行黏包处理
        ByteArray arrl = new ByteArray();
        arrl.Write(ba.Length);
        arrl.Write(ba.GetBuff());
        try
        {
            socket.Send(arrl.GetBuff());
        }
        catch(Exception e)
        {
            Debug.Log("网络错误，请重新登录" + e.Message);
        }
    }

    /// <summary>
    /// 缓存中的数据处理,收到消息后先解码
    /// </summary>
    private void OnData()
    {
        //长度解码
        byte[] result = LengthEncoding.Decode(ref cache);

        //长度解码为空，说明消息体不全，等待下条消息过来补全
        if (result == null)
        {
            isReading = false;
            return;
        }

        SocketModel messsage = (SocketModel)MessageEncoding.MessageDecode(result);

        if (messsage == null)
        {
            isReading = false;
            return;
        }

        //进行消息处理
        messages.Add(messsage);
        //尾递归 防止在消息处理过程中 有其他消息到达而没有经过处理
        OnData();
    }
}