#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			FightHandler.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 13:44:44Z
//
// 描述(Description):			FightHandler 战斗逻辑				
//
// **********************************************************************

#endregion

using System.Collections.Concurrent;
using System.Threading;
using GameProtocol.Dto;
using LOLServer.Logic.Selection;
using NetFrame;
using NetFrame.Auto;
using Tools;

namespace LOLServer.Logic.Fight
{
    public class FightHandler:AbsMutilHandler,IHandlerInterface
    {
        #region 字段定义
        /// <summary>
        /// 线程安全字典,防止数据竞争导致的脏数据，玩家所在战斗房间映射
        /// </summary>
        private ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// 房间id与模型的映射
        /// </summary>
        private ConcurrentDictionary<int, FightRoom> roomMap = new ConcurrentDictionary<int, FightRoom>();

        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        private ConcurrentStack<FightRoom> cacheRooms = new ConcurrentStack<FightRoom>();

        //房间ID自增器 需要线程安全
        private int index = 0;

        public int AddIndex
        {
            get { return Interlocked.Increment(ref index); }
        }
        public int ReduIndex
        {
            get { return Interlocked.Decrement(ref index); }
        }

        public int RestIndex
        {
            get { return Interlocked.Exchange(ref index, 0); }
        }

        #endregion
        
        public FightHandler()
        {
            EventUtil.createFight = Create;
            EventUtil.destroyFight = Destroy;
        }

        /// <summary>
        /// 房间创建 需要知道选择模型里的数据
        /// </summary>
        public void Create(SelectModel[] teamOne,SelectModel[] teamTwo)
        {
            FightRoom room;
            if(!cacheRooms.TryPop(out room))
            {
                room = new FightRoom();
                //添加唯一ID
                room.SetArea(AddIndex);
            }
            //房间数据初始化 
            room.Init(teamOne,teamTwo);
            foreach (SelectModel item in teamOne)
            {
                userRoom.TryAdd(item.UserID, room.GetArea());
            }
            foreach (SelectModel item in teamTwo)
            {
                userRoom.TryAdd(item.UserID, room.GetArea());
            }

            roomMap.TryAdd(room.GetArea(), room);
        }

        /// <summary>
        /// 战斗结束，移出房间
        /// </summary>
        /// <param name="roomID"></param>
        public void Destroy(int roomID)
        {
            FightRoom room;
            if (roomMap.TryRemove(roomID, out room))
            {
                int temp = 0;
                //移除角色和房间之间的绑定关系
                foreach (int item in room.teamOnes.Keys)
                {
                    userRoom.TryRemove(item, out temp);
                }
                foreach (int item in room.teamTwos.Keys)
                {
                    userRoom.TryRemove(item, out temp);
                }
                room.userList.Clear();
                room.teamOnes.Clear();
                room.teamTwos.Clear();
                //将房间丢进缓存队列，供下次选择使用
                cacheRooms.Push(room);
            }
        }

      
        public void ClientClose(UserToken token, string error)
        {
            //判断玩家是否在战斗中
            if (userRoom.ContainsKey(GetUserId(token)))
            {
                roomMap[userRoom[GetUserId(token)]].ClientClose(token, error);
                
            }
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            roomMap[userRoom[GetUserId(token)]].MessageReceive(token,message);
        }
    }
}