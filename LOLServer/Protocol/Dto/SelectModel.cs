#region

using System;

#endregion

namespace GameProtocol.Dto
{
    /// <summary>
    ///     选择模块DTO，所有序列化信息都必须添加序列化标签
    /// </summary>
    [Serializable]
    public class SelectModel
    {
        /// <summary>
        ///     用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        ///     用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     所选英雄
        /// </summary>
        public int Hero { get; set; }

        /// <summary>
        ///     是否进入
        /// </summary>
        public bool IsEnter { get; set; }

        /// <summary>
        ///     是否准备
        /// </summary>
        public bool IsReady { get; set; }
    }
}