using NetFrame;

namespace BIZ
{
    /// <summary>
    /// 事务接口层，账号管理事务
    /// </summary>
    public interface IAccountBIZ
    {
        /// <summary>
        /// 账号创建
        /// </summary>
        /// <param name="token">连接用户</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>返回创建结果 0 成功 1 账号重复 2 账号不合法 3 密码不合法</returns>
        int Create(UserToken token, string account, string password);

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="token">连接用户</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>返回登陆结果 0 成功 -1 账号不存在 -2 账号在线 -3 密码错误 -4 输入不合法 </returns>
        int Login(UserToken token, string account, string password);

        /// <summary>
        /// 客户端断开连接 下线
        /// </summary>
        /// <param name="token"></param>
        void Close(UserToken token);

        /// <summary>
        /// 获取账号ID
        /// </summary>
        /// <param name="token">连接的用户</param>
        /// <returns>返回用户登陆账号ID</returns>
        int Get(UserToken token);
    }
}