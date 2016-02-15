#region

using LOLServer.DAO.Model;
using NetFrame;

#endregion

namespace BIZ
{
    public interface IUserBiz
    {
        /// <summary>
        ///     创建召唤师
        /// </summary>
        /// <param name="token">连接信息</param>
        /// <param name="name">
        ///     名字
        /// </param>
        /// <returns></returns>
        bool CreateRole(UserToken token, string name);

        /// <summary>
        ///     获取连接对应的用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel Get(UserToken token);
        /// <summary>
        ///     通过账号获取对象，仅在初始验证登录时有效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel GetByAccount(UserToken token);

        /// <summary>
        ///     通过ID获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel Get(int id);

        /// <summary>
        ///     用户上线
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel OnLine(UserToken token);

        /// <summary>
        ///     用户下线
        /// </summary>
        /// <param name="token"></param>
        void OffLine(UserToken token);

        /// <summary>
        ///     根据ID获取连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken GetToken(int id);
    }
}