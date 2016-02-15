using BIZ;
using GameProtocol;
using GameProtocol.Dto;
using LOLServer.DAO.Model;
using NetFrame;
using NetFrame.Auto;
using Tools;

namespace LOLServer.Logic.User
{
    public class UserHandler:AbsHandlerBase,IHandlerInterface
    {
        private IUserBiz userBiz = BizFactory.userBiz;
        public void ClientClose(UserToken token, string error)
        {
            userBiz.OffLine(token);
        }

        
        public void MessageReceive(UserToken token, SocketModel message)
        {
            switch (message.Command)
            {
                case UserProtocol.CREATE_CREQ:
                    CreateRole(token, message.GetMessage<string>());
                    break;
                case UserProtocol.INFO_CREQ:
                    GetInfo(token);
                    break;
                case UserProtocol.ONLINE_CREQ:
                    OnLine(token);
                    break;
            }
        }

       
        private void CreateRole(UserToken token, string message)
        {
            ExecutorPool.Instance.Execute(delegate()
            {
                Write(token,UserProtocol.CREATE_SRES,userBiz.CreateRole(token,message));
            });
        }

        private void GetInfo(UserToken token)
        {
            ExecutorPool.Instance.Execute(delegate()
            {
                Write(token, UserProtocol.INFO_SRES, Convert(userBiz.GetByAccount(token)));
            });
        }

        private void OnLine(UserToken token)
        {
            ExecutorPool.Instance.Execute(delegate()
            {
                Write(token, UserProtocol.ONLINE_SRES, Convert(userBiz.OnLine(token)));
            });
        }

        private UserDTO Convert(UserModel user)
        {
            if (user == null) return null;
            return new UserDTO(user.ID, user.Name, user.Level, user.Exp, user.WinCount, user.LoseCount, user.RunCount,user.HeroList.ToArray());
        }

        public override byte GetType()
        {
            return Protocol.TYPE_USER;
        }
    }
}