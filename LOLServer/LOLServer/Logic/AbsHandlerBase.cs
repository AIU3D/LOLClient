using System;
using BIZ;
using LOLServer.DAO.Model;
using NetFrame;
using NetFrame.Auto;

namespace LOLServer.Logic
{
    /// <summary>
    /// 逻辑处理类的基类
    /// </summary>
    public class AbsHandlerBase
    {
        public  IUserBiz userBiz = BizFactory.userBiz;
        private byte type;
        private int area;

        public void SetArea(int area)
        {
            this.area = area;
        }

        public virtual int GetArea()
        {
            return area;
        }

        public void SetType(byte type)
        {
            this.type = type;
        }

        public new virtual byte GetType()
        {
            return type;
        }

        public UserModel GetUser(UserToken token)
        {
            return userBiz.Get(token);
        }

        public UserModel GetUser(int id)
        {
            return userBiz.Get(id);
        }

        public int GetUserId(UserToken token)
        {
            UserModel user = GetUser(token);
            if (user == null)
            {
                return -1;
            }
            return user.ID;
        }

        public UserToken GetToken(int id)
        {
            return userBiz.GetToken(id);
        }

        #region 通过连接对象发送

        public void Write(UserToken token, int command)
        {
            Write(token,command,null);
        }

        public void Write(UserToken token, int command, object message)
        {
            Write(token, GetArea(), command, message);
        }

        public void Write(UserToken token, int area, int command, object message)
        {
            Write(token, GetType(), GetArea(), command, message);
        }

        public void Write(UserToken token, byte type, int area, int command, object message)
        {
            //消息体的编码
            byte[] value = MessageEncoding.MessageEncode(CreateSocketModel(type, area, command, message));
            //长度编码，因为在服务器传入的时候进行了长度编码，所以在处理的时候也要进行长度编码
            value = LengthEncoding.Encode(value);
            token.Write(value);
        }

        #endregion

        #region 通过ID发送

        public void Write(int id, int command)
        {
            Write(id,command,null);
        }

        public void Write(int id, int command, object message)
        {
            Write(id,GetArea(),command,message);
        }

        public void Write(int id, int area, int command, object message)
        {
            Write(id,GetType(),area,command,message);
        }

        public void Write(int id, byte type, int area, int command, object message)
        {
            UserToken token = GetToken(id);
            if (token == null)
            {
                return;
            }
            else
            {
                Write(token, type, area, command, message);
            }
        }

        public void WriteToUsers(int[] users, byte type, int area, int command, object message)
        {
            byte[] value = MessageEncoding.MessageEncode(CreateSocketModel(type, area, command, message));
            value = LengthEncoding.Encode(value);
            foreach (int item in users)
            {
                UserToken token = userBiz.GetToken(item);
                if(token == null) continue;
                byte[] bs = new byte[value.Length]; //防治将元数据变更
                Array.Copy(value, 0, bs, 0, value.Length);
                token.Write(bs);
            }
        }

        #endregion

        public SocketModel CreateSocketModel(byte type, int area, int command, object message)
        {
            return new SocketModel(type, area, command, message);
        }

       
    }
}