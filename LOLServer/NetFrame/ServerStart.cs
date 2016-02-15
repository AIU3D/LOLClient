using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetFrame
{
    /// <summary>
    /// 写成一个类库，方便以后其他程序的调用
    /// 服务的开始类
    /// </summary>
    public class ServerStart
    {
        private Socket server;              //服务器Socket监听对象
        private int maxClient;              //客户端最大连接数
        private Semaphore acceptClient;     //限制可同时访问某一资源的线程数
        private UserTokenPool pool;         //客户端池

        public LengthEncode lengthEncode;         // 声明长度编码器的委托
        public LengthDecode lengthDecode;         // 声明长度解码器的委托
        public MessageEncode messageEncode;       // 声明消息编码器的委托
        public MessageDecode messageDecode;       // 声明消息解码器的委托

        public AbsHandlerCenter center;           //消息处理中心，必须有外部应用传入

        /// <summary>
        /// 初始化通信监听
        /// </summary>
        /// <param name="max">客户端最大连接数</param>
        
        public ServerStart(int max)
        {

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);   //实例化监听对象
            maxClient = max;                                                                        //设定最大连接数
        }

        /// <summary>
        /// 服务器运行
        /// </summary>
        /// <param name="port"></param>
        public void Start(int port)
        {
            InitTokenPool(maxClient);
            
            // 监听当前服务器网卡所有可用IP地址的Port端口
            //一般分为内网IP，外网IP和本机IP
            try
            {
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                //一次最多可以监听10个Ip，置于监听状态
                server.Listen(10);
                //开始连接
                StartAccept(null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="max">连接数</param>
        private void InitTokenPool(int max)
        {
            pool = new UserTokenPool(max); //创建连接池
            acceptClient = new Semaphore(max, max); //连接信号量，初始化数量和最大连接数量

            //在初始化的时候就将连接对象依次创建，好处：省去了连接时候创建，断开时销毁的过程，坏处：服务器连接空余较多
            for (int i = 0; i < max; i++)
            {
                UserToken token = new UserToken();
                //初始化token信息
                token.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                token.sendSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                token.lengthEncode = lengthEncode;
                token.lengthDecode = lengthDecode;
                token.messageEncode = messageEncode;
                token.messageDecode = messageDecode;
                token.sendProcess = ProcessSend;
                token.closeProcess = ClientClose;
                token.center = center;

                pool.Push(token);
            }
        }
        /// <summary>
        /// 开始客户端连接监听
        /// </summary>
        /// <param name="e"></param>
        public void StartAccept(SocketAsyncEventArgs e)
        {
            //如果当前传入为空，说明调用新的客户端连接监听事件，否则的话移除当前客户端连接
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);
            }
            else
            {
                e.AcceptSocket = null;
            }
            //信号量-1
            acceptClient.WaitOne();
            bool result = server.AcceptAsync(e);

            //判断异步事件是否挂起，没挂起说明立刻执行完成，直接处理事件 否则会在处理完成后触发Accept_Completed事件
            //异步完成事件在直接完成是不会被触发，所以要在这里判断下是否直接完成
            if (!result)
            {
                ProcessAccept(e);
            }
        }

        /// <summary>
        /// 连接完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        /// <summary>
        /// 处理连接事件
        /// </summary>
        /// <param name="e"></param>
        public void ProcessAccept(SocketAsyncEventArgs e)
        {
            //从连接对象池中取出一个连接对象，供新用户使用
            UserToken token = pool.Pop();
            token.conn = e.AcceptSocket;
            // TODO 通知应用层有客户端连接
            center.ClientConnect(token);
            //开启消息到达监听
            StartReceive(token);

            //释放当前异步对象
            StartAccept(e);
        }

        /// <summary>
        /// 开始接收数据
        /// </summary>
        /// <param name="token"></param>
        public void StartReceive(UserToken token)
        {
            try
            {
                
                //用户连接对象，开启异步数据接收,必须设置接收对象的缓冲区大小
                bool result = token.conn.ReceiveAsync(token.receiveSAEA);
            
                //异步事件是否挂起
                if (!result)
                {
                    ProcessReceive(token.receiveSAEA);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        /// <summary>
        /// 发送或接收完成时调用的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation == SocketAsyncOperation.Receive)
            {
                ProcessReceive(e);
            }
            else
            {
                ProcessSend(e);
            }
        }

        /// <summary>
        /// 处理接收事件
        /// </summary>
        /// <param name="e"></param>
        public void ProcessReceive(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            
            //判断网络消息接收是否成功
            if (token.receiveSAEA.BytesTransferred > 0 && token.receiveSAEA.SocketError == SocketError.Success)
            {
                byte[] message = new byte[token.receiveSAEA.BytesTransferred];
                Buffer.BlockCopy(token.receiveSAEA.Buffer, 0, message, 0, token.receiveSAEA.BytesTransferred);
                //处理接收到的消息
                token.Receive(message);
                //继续接受消息
                StartReceive(token);
            }
            else
            {
                //如果消息长度为0，说明客户端主动断开连接，这是默认规则
                if (token.receiveSAEA.SocketError != SocketError.Success)
                {
                    ClientClose(token,token.receiveSAEA.SocketError.ToString());
                }
                else
                {
                    ClientClose(token, "客户端主动断开连接");
                }
            }
        }

        /// <summary>
        /// 处理发送事件
        /// </summary>
        /// <param name="e"></param>
        public void ProcessSend(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if (e.SocketError != SocketError.Success)
            {
                ClientClose(token,e.SocketError.ToString());
            }
            else
            {
                //消息发送成功，回调成功
                token.Writed();
            }
        }

        /// <summary>
        /// 客户端断开连接
        /// </summary>
        /// <param name="token">断开连接的用户对象</param>
        /// <param name="error"断开连接的错误编码></param>
        public void ClientClose(UserToken token, string error)
        {
            if (token.conn != null)
            {
                lock (token)
                {
                    //通知应用层面，客户端断开连接
                    center.ClientClose(token, error);
                    token.Close();
                    //返回用户池
                    pool.Push(token);
                    //加回一个信号量，供其他用户使用
                    acceptClient.Release();
                }
            }
        }
    }
}
