using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace NetFrame
{
    /// <summary>
    /// 用户连接信息对象
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// 用户连接
        /// </summary>
        public Socket conn;

        /// <summary>
        /// 用户异步接收网络数据对象
        /// </summary>
        public SocketAsyncEventArgs receiveSAEA;

        /// <summary>
        /// 用户异步发送网络数据对象
        /// </summary>
        public SocketAsyncEventArgs sendSAEA;

        /// <summary>
        /// 处理收到的消息，处理黏包问题
        /// </summary>
        private List<byte> cache = new List<byte>();

        private bool isReading = false;
        private bool isWriting = false;

        /// <summary>
        /// 发送消息队列
        /// </summary>
        private Queue<byte[]> writeQueue = new Queue<byte[]>(); 
        
        /// <summary>
        /// 声明长度编码器的委托
        /// </summary>
        public LengthEncode lengthEncode;

        /// <summary>
        /// 声明长度解码器的委托
        /// </summary>
        public LengthDecode lengthDecode;

        /// <summary>
        /// 声明消息编码器的委托
        /// </summary>
        public MessageEncode messageEncode;

        /// <summary>
        /// 声明消息解码器的委托
        /// </summary>
        public MessageDecode messageDecode;

        /// <summary>
        /// 发送处理的委托
        /// </summary>
        public SendProcess sendProcess;

        /// <summary>
        /// 关闭的消息处理
        /// </summary>
        public ColseProcess closeProcess;

        /// <summary>
        /// 消息处理中心
        /// </summary>
        public AbsHandlerCenter center;

        public UserToken()
        {
            receiveSAEA = new SocketAsyncEventArgs();
            sendSAEA = new SocketAsyncEventArgs();
            receiveSAEA.UserToken = this;
            sendSAEA.UserToken = this;

            //设置接收对象的缓冲区大小
            receiveSAEA.SetBuffer(new byte[1024], 0, 1024);
        }

        /// <summary>
        /// 消息接收，处理黏包等问题,消息太大，分段发送回产生黏包问题
        /// </summary>
        /// <param name="buff"></param>
        public void Receive(byte[] buff)
        {
            //将消息写入缓存
            cache.AddRange(buff);
            if (!isReading)
            {
                isReading = true;
                OnData();
            }
        }

        /// <summary>
        /// 缓存中的数据处理,收到消息后先解码
        /// </summary>
        private void OnData()
        {
            //解码消息存储对象
            byte[] buff = null;
            //当黏包解码器不为空时，进行黏包处理
            if (lengthDecode != null)
            {
                buff = lengthDecode(ref cache);
                //消息未接收全，退出数据处理，等待下次到达
                if (buff == null)
                {
                    isReading = false;
                    return;
                }
            }
            else
            {
                //缓存区中没有数据，直接跳出消息处理，等待下次消息到达，防止尾递归没有数据时造成错误
                if (cache.Count == 0)
                {
                    isReading = false;
                    return;
                }
                buff = cache.ToArray();
                cache.Clear();
            }

            //消息解码器必须有
            if (messageDecode == null)
            {
                throw new Exception(" message decode process is null");
            }
            //进行消息的反序列化
            object message = messageDecode(buff);

            //TODO 通知应用层有消息到达
            center.MessageReceive(this, message);
            //尾递归 防止在消息处理过程中 有其他消息到达而没有经过处理
            OnData();
        }

        /// <summary>
        /// 消息发送处理消息队列或拆包问题
        /// </summary>
        public void Write(byte[] value)
        {
            if (conn == null)
            {
                //此链接已断开
                closeProcess(this, "连接已断开");
                return;
            }
            writeQueue.Enqueue(value);
            if (!isWriting)
            {
                isWriting = true;
                OnWrite();
            }
        }

        /// <summary>
        /// 发送数据先编码
        /// </summary>
        public void OnWrite()
        {
            //判断消息队列是否有消息
            if (writeQueue.Count == 0)
            {
                isWriting = false;
                return;
            }

            //取出第一条消息
            byte[] buff = writeQueue.Dequeue();
            //设置消息发送异步对象的发送数据缓冲区数据
            sendSAEA.SetBuffer(buff, 0, buff.Length);
            //开启异步发送
            bool result = conn.SendAsync(sendSAEA);
            //是否挂起
            if (!result)
            {
                sendProcess(sendSAEA);
            }
        }

        /// <summary>
        /// 数据发送成功时调用
        /// </summary>
        public  void Writed()
        {
            //尾递归，和OnData一样
            OnWrite();
        }

        /// <summary>
        /// 用户连接关闭
        /// </summary>
        public void Close()
        {
            try
            {
                writeQueue.Clear();
                cache.Clear();
                isReading = false;
                isWriting = false;
                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
                conn = null;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        
    }
}