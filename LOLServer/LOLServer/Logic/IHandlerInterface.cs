using NetFrame;
using NetFrame.Auto;

namespace LOLServer.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHandlerInterface
    {
        void ClientClose(UserToken token, string error);
        void MessageReceive(UserToken token, SocketModel message);
    }
}