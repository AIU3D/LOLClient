using System.CodeDom;
using System.Threading;

namespace Tools
{
    /// <summary>
    /// 单线程处理对象，将所有事物处理的调用，通过此处调用 
    /// </summary>
    public class ExecutorPool
    {
        /// <summary> 线程同步锁 </summary>
        private Mutex tex = new Mutex();

        private static ExecutorPool instance;

        public static ExecutorPool Instance
        {
            get
            {
                if (instance == null)
                    instance = new ExecutorPool();
                return instance;
            }
        }

        /// <summary>
        /// 单线程处理逻辑，使用锁将线程锁住，执行完毕后，释放
        /// </summary>
        /// <param name="ed"></param>
        public void Execute(ExecutorDelegate ed)
        {
            lock (this)
            {
                tex.WaitOne();
                ed();
                tex.ReleaseMutex();
            }
        }
    }
}
