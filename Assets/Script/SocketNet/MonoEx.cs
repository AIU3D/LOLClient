using UnityEngine;

public static class MonoEx
{
    /// <summary>
    /// 扩展monobehaviout发送消息方法
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="type"></param>
    /// <param name="area"></param>
    /// <param name="command"></param>
    /// <param name="message"></param>
    public static void Wirte(this MonoBehaviour mono, byte type, int area, int command, object message)
    {
        NetIO.Instance.Write(type, area, command, message);
    }
}