using LOLServer.Cache;
using NetFrame;

namespace BIZ.Impl
{
    /// <summary>
    /// 账号管理的具体实现
    /// </summary>
    public class AccountBIZ:IAccountBIZ
    {
        private IAccountCache accountCache = CacheFactory.accountCache;
        public int Create(UserToken token, string account, string password)
        {
            if (accountCache.HasAccount(account)) return 1;
            accountCache.Add(account, password);
            return 0;
        }

        public int Login(UserToken token, string account, string password)
        {
            //账号密码为空，输入不合法
            if (account == null || password == null) return -4;
            //判断账号是否存在
            if (!accountCache.HasAccount(account)) return -1;
            //判断账号是否在线
            if (accountCache.IsOnline(account)) return -2;
            //判断账号密码是否匹配
            if (!accountCache.Match(account, password)) return -3;
            accountCache.Online(token, account);
            return 0;
        }

        public void Close(UserToken token)
        {
            accountCache.Offline(token);
        }

        public int Get(UserToken token)
        {
            return accountCache.GetID(token);
        }
    }
}