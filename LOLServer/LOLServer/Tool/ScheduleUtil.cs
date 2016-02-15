using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Tools
{
    public delegate void TimerEvent();
    /// <summary>
    /// 时刻表工具
    /// </summary>
    public class ScheduleUtil
    {
        private static ScheduleUtil instance;

        public static ScheduleUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScheduleUtil();
                }
                return instance;
            }
        }

        private Timer timer;

        private int index = 0;

        #region 自增

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

        //等待执行的任务列表
        private Dictionary<int, TimerTaskModel> missions = new Dictionary<int, TimerTaskModel>();

        //等待移除列表，并不是实时移除
        private List<int> removeList = new List<int>(); 
        private ScheduleUtil()
        {
            timer = new Timer(200);
            timer.Elapsed += callback; //完成时回调
            timer.Start();
        }

        void callback(object sender, ElapsedEventArgs e)
        {
            lock (missions)
            {
                lock (removeList)
                {
                    foreach (int item in removeList)
                    {
                        missions.Remove(item);
                    }
                    removeList.Clear();

                    foreach (TimerTaskModel item in missions.Values)
                    {
                        if (item.Time <= DateTime.Now.Ticks)
                        {
                            item.Run();
                            removeList.Add(item.Id);
                        }
                    }
                }
            }
        }

        public int Schedule(TimerEvent task,long delay)
        {
            return Schedulemms(task,delay*1000*1000);
        }

        /// <summary>
        /// 微秒级的时间
        /// </summary>
        /// <param name="task"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private int Schedulemms(TimerEvent task, long delay)
        {
            lock (missions)
            {
                  int id = AddIndex;
            TimerTaskModel model = new TimerTaskModel(id,DateTime.Now.Ticks + delay,task);
            missions.Add(id,model);
            return id;
            }
          
        }

        /// <summary>
        /// 任务删除
        /// </summary>
        /// <param name="id"></param>
        public void RemoveMission(int id)
        {
            lock (removeList)
            {
                removeList.Add(id);                
            }
        }

        public int Schedule(TimerEvent task, DateTime time)
        {
            long t = time.Ticks - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return Schedulemms(task,t);
        }

        public int TimeSchedule(TimerEvent task, long time)
        {
            long t = time - DateTime.Now.Ticks;
            t = Math.Abs(t);
            return Schedulemms(task, t);
        }


    }
}