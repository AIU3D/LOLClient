using System;
using System.Collections.Generic;
using NetFrame;
using NetFrame.Auto;

namespace LOLServer.Logic
{
    /// <summary>
    /// 消息群发
    /// </summary>
    public class AbsMutilHandler:AbsHandlerBase
    {
        public List<UserToken> userList = new List<UserToken>();

        /// <summary>
        /// 用户进入当前子模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Enter(UserToken token)
        {
            if (userList.Contains(token))
            {
                return false;
            }
            else
            {
                userList.Add(token);
                return true;
            }
        }

        /// <summary>
        /// 是否在此子模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsEntered(UserToken token)
        {
            return userList.Contains(token);
        }

        /// <summary>
        /// 用户离开当前子模块
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Leave(UserToken token)
        {
            if (userList.Contains(token))
            {
                userList.Remove(token);
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 群发

      
        /// <summary>
        /// 广播发送
        /// </summary>
        /// <param name="command">协议</param>
        /// <param name="message">消息体</param>
        public void Brocast(int command, object message ,UserToken exToken = null)
        {
            Brocast(GetArea(), command, message, exToken);
        }

        public void Brocast(int area, int command, object message, UserToken exToken = null)
        {
            Brocast(GetType(), area, command, message, exToken);
        }

        public void Brocast(byte type, int area, int command, object message, UserToken exToken = null)
        {
            byte[] value = MessageEncoding.MessageEncode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.Encode(value);
            foreach (UserToken item in userList)
            {
                if (item == exToken) continue;
                byte[] bs = new byte[value.Length]; //防治将元数据变更
                Array.Copy(value, 0, bs, 0, value.Length);
                item.Write(bs);
            }
        }

        #endregion

    }
}