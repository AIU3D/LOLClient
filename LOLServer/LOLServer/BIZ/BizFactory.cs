using BIZ.Impl;
using BIZ.User.Impl;

namespace BIZ
{
    /// <summary>
    /// 事务工厂，避免每次访问都创建一个实例，就将实例声明为静态的
    /// </summary>
    public class BizFactory
    {
        /// <summary> 不允许被更改 </summary>
        public static readonly IAccountBIZ accountBiz;

        public static readonly IUserBiz userBiz;
        /// <summary>
        /// 静态构造函数，在开始时就自动创建
        /// </summary>
        static BizFactory()
        {
            accountBiz = new AccountBIZ();
            userBiz = new UserBiz();
        }
    }
}