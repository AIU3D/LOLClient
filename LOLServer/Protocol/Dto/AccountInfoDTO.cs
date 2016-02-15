using System;

namespace GameProtocol.Dto
{
    /// <summary>
    /// DTO传输模型必须添加序列化标签
    /// </summary>
    [Serializable]
    public class AccountInfoDTO
    {
        public string account;
        public string password;
    }
}