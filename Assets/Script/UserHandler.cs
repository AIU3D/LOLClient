using GameProtocol;
using GameProtocol.Dto;
using NetFrame.Auto;
using UnityEngine;

namespace Assets.Script
{
    public class UserHandler:MonoBehaviour,IHandler
    {
        public void MessageReceive(SocketModel model)
        {
            switch (model.Command)
            {
                case UserProtocol.INFO_SRES:
                    GetInfo(model.GetMessage<UserDTO>());
                    break;
                case UserProtocol.CREATE_SRES:
                    CreateRole(model.GetMessage<bool>());
                    break;
                case UserProtocol.ONLINE_SRES:
                    OnLine(model.GetMessage<UserDTO>());
                    break;
            }
        }

        private void GetInfo(UserDTO user)
        {
            if (user == null)
            {
                //显示创建面板
                SendMessage("OpenCreatePlane");
            }
            else
            {
                //向服务器申请登录
                this.Wirte(Protocol.TYPE_USER,0,UserProtocol.ONLINE_CREQ,null);
            }
        }

        private void OnLine(UserDTO user)
        {
            GameData.user = user;
            //移除遮罩
            SendMessage("CloseMask");
            //刷新UI数据
            SendMessage("RefreshView");
        }

        private void CreateRole(bool value)
        {
            if (value)
            {
                WarningManager.errors.Add(new WarningModel("创建成功", () =>
                {
                    //关闭创建面板
                    SendMessage("CloseCreatePlane");
                    //直接申请登录
                    this.Wirte(Protocol.TYPE_USER, 0, UserProtocol.ONLINE_CREQ, null);
                }));
                
            }
            else
            {
                //刷新创建面板
                SendMessage("OpenCreatePlane");

            }
        }
    }
}