using NetFrame;

namespace LOLServer.Cache
{
    /// <summary>
    /// 账号缓存层接口
    /// </summary>
    public interface IAccountCache
    {
        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        bool HasAccount(string account);

        /// <summary>
        /// 账号密码是否匹配
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        bool Match(string account, string password);

        /// <summary>
        /// 账号是否在线
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns></returns>
        bool IsOnline(string account);

       /// <summary>
       /// 当前连接对象多对应的ID
       /// </summary>
       /// <param name="token">连接对象</param>
       /// <returns></returns>
        int GetID(UserToken token);

        /// <summary>
        /// 账号上线
        /// </summary>
        /// <param name="token">连接对象</param>
        /// <param name="account">账号</param>
        void Online(UserToken token, string account);

        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="token">连接对象</param>
        void Offline(UserToken token);

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        void Add(string account, string password);
    }
}