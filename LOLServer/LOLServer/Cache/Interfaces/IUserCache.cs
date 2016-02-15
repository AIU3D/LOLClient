#region

using LOLServer.DAO.Model;
using NetFrame;

#endregion

namespace LOLServer.Cache
{
    public interface IUserCache
    {
        /// <summary>
        ///     创建角色
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool CreateRole(UserToken token, string name, int accountID);

        /// <summary>
        ///     是否拥有角色
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool HasRole(UserToken token);

        /// <summary>
        ///     通过账号ID判断是否拥有角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool HasByAccountId(int id);

        /// <summary>
        ///     获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel GetUser(UserToken token);

        /// <summary>
        ///     获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetUser(int id);

        /// <summary>
        ///     用户上线
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserModel OnLine(UserToken token, int id);

        /// <summary>
        ///     用户下线
        /// </summary>
        /// <param name="token"></param>
        void OffLine(UserToken token);

        /// <summary>
        ///     通过id 获得连接信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserToken GetToken(int id);

        /// <summary>
        ///     通过账号ID获得角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserModel GetByAccountID(int id);

        /// <summary>
        ///     角色是否在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsOnLine(int id);
    }
}