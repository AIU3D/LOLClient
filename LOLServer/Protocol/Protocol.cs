namespace GameProtocol
{
    /// <summary>
    /// 协议数据
    /// </summary>
    public class Protocol
    {
        /// <summary>
        /// 登录模块
        /// </summary>
        public const byte TYPE_LOGIN = 0;

        /// <summary>
        /// 用户模块
        /// </summary>
        public const byte TYPE_USER = 1;

        /// <summary>
        /// 用户匹配
        /// </summary>
        public const byte TYPE_MATCH = 2;

        /// <summary>
        /// 英雄选择
        /// </summary>
        public const byte TYPE_SELECT = 3;

        /// <summary>
        /// 战斗模块
        /// </summary>
        public const byte TYPE_FIGHT = 4;
    }
}