using LOLServer.Cache.impl;

namespace LOLServer.Cache
{
    public class CacheFactory
    {
        public static readonly IAccountCache accountCache;
        public static readonly IUserCache userCache;
        static CacheFactory()
        {
            accountCache = new AccountCache();
            userCache = new UserCache();
        }
    }
}