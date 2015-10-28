using GameProtocol;
using NetFrame.Auto;
using UnityEngine;


public class LoginHandler : MonoBehaviour, IHandler
{
    public void MessageReceive(SocketModel model)
    {
//        WarningManager.errors.Add(new WarningModel(model.Message+""));
        switch (model.Command)
        {
            case LoginProtocol.LOGIN_SRES:
                Login(model.GetMessage<int>());
                break;
            case LoginProtocol.REG_SRES:
                Reg(model.GetMessage<int>());
                break;
        }
    }

    /// <summary>
    /// 登录返回处理
    /// </summary>
    private void Login(int value)
    {
        SendMessage("OpenLoginBtn");
        switch (value)
        {
            case 0:
                //加载游戏主场景
                Application.LoadLevel(1);
                break;
            case -1:
                WarningManager.errors.Add(new WarningModel("账号不存在"));
                break;
            case -2:
                WarningManager.errors.Add(new WarningModel("账号在线"));
                break;
            case -3:
                WarningManager.errors.Add(new WarningModel("密码错误"));
                break;
            case -4:
                WarningManager.errors.Add(new WarningModel("输入不合法"));
                break;
        }
    }

    /// <summary>
    /// 注册返回处理
    /// </summary>
    private void Reg(int value)
    {
        switch (value)
        {
            case 0:
                WarningManager.errors.Add(new WarningModel("注册成功"));
                break;
            case -1:
                WarningManager.errors.Add(new WarningModel("注册失败，账号存在"));
                break;
           
        }
    }
}