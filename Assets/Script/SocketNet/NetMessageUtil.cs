using GameProtocol;
using NetFrame.Auto;
using UnityEngine;


public class NetMessageUtil : MonoBehaviour
{
    private IHandler login;

    void Start()
    {
        login = GetComponent<LoginHandler>();
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
        }
    }
}