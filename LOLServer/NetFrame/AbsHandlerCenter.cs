namespace NetFrame
{
    /// <summary>
    /// 消息处理中心，主要处理连接，接收和断开消息
    /// </summary>
    public abstract class AbsHandlerCenter
    {
        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <param name="token">连接对象</param>
        public abstract void ClientConnect(UserToken token);

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        /// <param name="token">发送消息的连接对象</param>
        /// <param name="message">消息内容</param>
        public abstract void MessageReceive(UserToken token, object message);

        /// <summary>
        /// 客户端断开
        /// </summary>
        /// <param name="token">断开的客户端对象</param>
        /// <param name="error">断开的错误消息</param>
        public abstract void ClientClose(UserToken token, string error);
    }
}