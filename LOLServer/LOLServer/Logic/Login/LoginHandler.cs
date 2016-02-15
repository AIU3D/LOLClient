using System;
using BIZ;
using GameProtocol;
using GameProtocol.Dto;
using NetFrame;
using NetFrame.Auto;
using Tools;

namespace LOLServer.Logic.Login
{
    public class LoginHandler:AbsHandlerBase,IHandlerInterface
    {
        private IAccountBIZ accountBIZ = BizFactory.accountBiz;
        public void ClientClose(UserToken token, string error)
        {
            ExecutorPool.Instance.Execute(delegate()
            {
                accountBIZ.Close(token);
            });
        }

     

        public void MessageReceive(UserToken token, SocketModel message)
        {
           //登录模块没有子模块，所以直接判断命令协议
            switch (message.Command)
            {
                case LoginProtocol.LOGIN_CREQ:
                    Login(token, message.GetMessage<AccountInfoDTO>());
                    break;
                case LoginProtocol.REG_CREQ:
                    Reg(token, message.GetMessage<AccountInfoDTO>());
                    break;

            }
        }

        /// <summary>
        /// 处理登录
        /// </summary>
        public void Login(UserToken token, AccountInfoDTO value)
        {
            ExecutorPool.Instance.Execute(
                delegate()
                {
                    int result = accountBIZ.Login(token, value.account, value.password);
                    Write(token, LoginProtocol.LOGIN_SRES, result);
                });
        }

        /// <summary>
        /// 处理注册
        /// </summary>
        public void Reg(UserToken token, AccountInfoDTO value)
        {
            ExecutorPool.Instance.Execute(
                delegate()
                {
                    int result = accountBIZ.Create(token, value.account, value.password);
                    Write(token, LoginProtocol.REG_SRES, result);
                });
        }

        public override byte GetType()
        {
            return Protocol.TYPE_LOGIN;
        }
    }
}