using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using NetFrame;
using NetFrame.Auto;
using Tools;

namespace LOLServer.Logic.Selection
{

  

    /// <summary>
    /// 选人逻辑
    /// </summary>
    public class SelectHandler : AbsHandlerBase, IHandlerInterface
    {
        #region 字段定义
        /// <summary>
        /// 线程安全字典,防止数据竞争导致的脏数据，玩家所在匹配房间映射
        /// </summary>
        private ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// 房间id与模型的映射
        /// </summary>
        private ConcurrentDictionary<int, SelectRoom> roomMap = new ConcurrentDictionary<int, SelectRoom>();

        /// <summary>
        /// 回收利用过的房间对象再次利用，减少gc性能开销
        /// </summary>
        private ConcurrentStack<SelectRoom> cacheRooms = new ConcurrentStack<SelectRoom>();

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

        public SelectHandler()
        {
            EventUtil.createSelect = Create;
            EventUtil.destroySelect = Destroy;
        }
        public void ClientClose(UserToken token, string error)
        {
            int userID = GetUserId(token);
            if (userRoom.ContainsKey(userID))
            {
                int roomID;
                userRoom.TryRemove(userID, out roomID);
                if (roomMap.ContainsKey(roomID))
                {
                    roomMap[roomID].ClientClose(token,error);
                }
            }
        }

        public void MessageReceive(UserToken token, SocketModel message)
        {
            int userID = GetUserId(token);
            if (userRoom.ContainsKey(userID))
            {
                int roomID = userRoom[userID];
                if (roomMap.ContainsKey(roomID))
                {
                    roomMap[roomID].MessageReceive(token,message);
                }
            }

            //两种方法都可以
//            if (roomMap.ContainsKey(message.Area))
//            {
//                roomMap[message.Area].MessageReceive(token,message);
//            }
        }

        public void Create(List<int> teamOne, List<int> teamTwo)
        {
            SelectRoom room;
            if(!cacheRooms.TryPop(out room))
            {
                room = new SelectRoom();
                //添加唯一ID
                room.SetArea(AddIndex);
            }
            //房间数据初始化 
            room.Init(teamOne,teamTwo);
            foreach (int item in teamOne)
            {
                userRoom.TryAdd(item, room.GetArea());
            }
            foreach (int item in teamTwo)
            {
                userRoom.TryAdd(item, room.GetArea());
            }

            roomMap.TryAdd(room.GetArea(), room);
        }

        public void Destroy(int roomID)
        {
            SelectRoom room;
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
                room.readyList.Clear();
                room.teamOnes.Clear();
                room.teamTwos.Clear();
                //将房间丢进缓存队列，供下次选择使用
                cacheRooms.Push(room);
            }
        }
    }
}