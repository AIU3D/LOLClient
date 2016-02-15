#region

using LOLServer.Cache;
using LOLServer.DAO.Model;
using NetFrame;

#endregion

namespace BIZ.User.Impl
{
    public class UserBiz : IUserBiz
    {
        private readonly IAccountBIZ accountBiz = BizFactory.accountBiz;
        private readonly IUserCache userCache = CacheFactory.userCache;

        public bool CreateRole(UserToken token, string name)
        {
            //账号是否登录 获取账号ID
            int accountID = accountBiz.Get(token);
            if (accountID == -1)
            {
                return false;
            }
            //判断当前是否已经拥有角色
            if (userCache.HasByAccountId(accountID))
            {
                return false;
            }

            userCache.CreateRole(token, name, accountID);
            return true;
        }

        public UserModel Get(UserToken token)
        {
            return userCache.GetUser(token);
        }

        public UserModel GetByAccount(UserToken token)
        {
            int accountID = accountBiz.Get(token);
            return accountID == -1 ? null : userCache.GetByAccountID(accountID);
        }

        public UserModel Get(int id)
        {
            return userCache.GetUser(id);
        }

        public UserModel OnLine(UserToken token)
        {
            int accountID = accountBiz.Get(token);
            if (accountID == -1)
            {
                return null;
            }

            UserModel user = userCache.GetByAccountID(accountID);
            if (userCache.IsOnLine(user.ID))
            {
                return null;
            }
            userCache.OnLine(token, user.ID);
            return user;
        }

        public void OffLine(UserToken token)
        {
            userCache.OffLine(token);
        }

        public UserToken GetToken(int id)
        {
            return userCache.GetToken(id);
        }
    }
}