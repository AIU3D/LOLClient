using Assets.Script;
using Assets.Script.Fight;
using GameProtocol;
using NetFrame.Auto;
using UnityEngine;


public class 
    NetMessageUtil : MonoBehaviour
{
    private IHandler login;
    private IHandler user;
    private IHandler match;
    private IHandler select;
    private IHandler fight;
    void Start()
    {
        login = GetComponent<LoginHandler>();
        user = GetComponent<UserHandler>();
        match = GetComponent<MatchHandler>();
        select = GetComponent<SelectHandler>();
        fight = GetComponent<FightHandler>();
    }
    void Update()
    {
        while (NetIO.Instance.messages.Count > 0)
        {
            SocketModel model = NetIO.Instance.messages[0];
            StartCoroutine("MessageReceive", model);
            NetIO.Instance.messages.RemoveAt(0);
           
        }
    }

    /// <summary>
    /// 处理消息接收数据
    /// </summary>
    /// <param name="model"></param>
    void MessageReceive(SocketModel model)
    {
        switch (model.Type)
        {
            case Protocol.TYPE_LOGIN:
                login.MessageReceive(model);
                break;
            case Protocol.TYPE_USER:
                user.MessageReceive(model);
                break;
            case Protocol.TYPE_MATCH:
                match.MessageReceive(model);
                break;
            case Protocol.TYPE_SELECT:
                select.MessageReceive(model);
                break;
            case Protocol.TYPE_FIGHT:
                fight.MessageReceive(model);
                break;
        }
    }
}