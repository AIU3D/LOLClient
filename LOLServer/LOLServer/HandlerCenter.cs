using System;
using GameProtocol;
using LOLServer.Logic;
using LOLServer.Logic.Fight;
using LOLServer.Logic.Login;
using LOLServer.Logic.Match;
using LOLServer.Logic.Selection;
using LOLServer.Logic.User;
using NetFrame;
using NetFrame.Auto;

namespace LOLServer
{
    /// <summary>
    /// 消息处理中心
    /// </summary>
    public class HandlerCenter:AbsHandlerCenter
    {
        private IHandlerInterface login;
        private IHandlerInterface user;
        private IHandlerInterface match;
        private IHandlerInterface select;
        private IHandlerInterface fight;
        public HandlerCenter()
        {
            Console.WriteLine("有客户端断开了连接");
            login = new LoginHandler();
            user = new UserHandler();
            match = new MatchHandler();
            select = new SelectHandler();
            fight = new FightHandler();
        }

        public override void ClientConnect(UserToken token)
        {
            Console.WriteLine("有客户端连接");
        }

        public override void MessageReceive(UserToken token, object message)
        {
            SocketModel model = message as SocketModel;

            switch (model.Type)
            {
                case Protocol.TYPE_LOGIN:
                    login.MessageReceive(token, model);
                    break;
                case Protocol.TYPE_USER:
                    user.MessageReceive(token,model);
                    break;
                case Protocol.TYPE_MATCH:
                    match.MessageReceive(token, model);
                    break;
                case Protocol.TYPE_SELECT:
                    select.MessageReceive(token, model);
                    break;
                case Protocol.TYPE_FIGHT:
                    fight.MessageReceive(token, model);
                    break;
                default:
                    //未知模块可能是客户端作弊了
                    break;
            }
        }

        public override void ClientClose(UserToken token, string error)
        {
            //user连接关闭方法，一定要放在逻辑单元后面
            //其他逻辑单元需要通过user绑定数据来进行内存清理
            //如果先清除了绑定关系，其他模块无法获取角色数据会导致无法清理
            Console.WriteLine("客户端断开连接");
            select.ClientClose(token,error);
            match.ClientClose(token,error);
            user.ClientClose(token,error);
            login.ClientClose(token, error);
            fight.ClientClose(token,error);
        }
    }
}