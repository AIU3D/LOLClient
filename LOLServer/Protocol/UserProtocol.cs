namespace GameProtocol
{
    /// <summary>
    ///     用户协议
    /// </summary>
    public class UserProtocol
    {
        /// <summary>
        ///     获取自身数据
        /// </summary>
        public const int INFO_CREQ = 0;

        /// <summary>
        ///     返回自身数据
        /// </summary>
        public const int INFO_SRES = 1;

        /// <summary>
        ///     申请创建角色
        /// </summary>
        public const int CREATE_CREQ = 2;

        /// <summary>
        ///     返回创建结果
        /// </summary>
        public const int CREATE_SRES = 3;

        /// <summary>
        ///     用户上线
        /// </summary>
        public const int ONLINE_CREQ = 4;

        /// <summary>
        ///     返回用户上线结果
        /// </summary>
        public const int ONLINE_SRES = 5;
    }
}