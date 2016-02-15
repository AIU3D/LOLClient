using System.Collections.Generic;

namespace NetFrame
{
    /// <summary>
    /// 限制玩家数量
    /// </summary>
    public class UserTokenPool
    {
        private Stack<UserToken> pool;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="max">最大客户端连接数</param>
        public UserTokenPool(int max)
        {
            pool = new Stack<UserToken>(max);
        }

        /// <summary>
        /// 获得池的大小
        /// </summary>
        public int Size
        {
            get { return pool.Count; }
        }

        /// <summary>
        /// 创建连接的时候取出一个连接对象
        /// </summary>
        public UserToken Pop()
        {
            return pool.Pop();
        }

        /// <summary>
        /// 插入一个连接对象 释放连接的时候
        /// </summary>
        public void Push(UserToken token)
        {
            if(token != null)
                pool.Push(token);
        }
    }
}