using GameProtocol;
using NetFrame.Auto;
using UnityEngine;

public class MatchHandler:MonoBehaviour,IHandler
{
    public void MessageReceive(SocketModel model)
    {
        if (model.Command == MatchProtocol.ENTER_DELECT_BRO)
        {
            Application.LoadLevel(2);
        }
    }
}