namespace Tools
{
    /// <summary>
    ///     时间任务模型
    /// </summary>
    public class TimerTaskModel
    {
        /// <summary>
        ///     任务逻辑
        /// </summary>
        private TimerEvent execute;

        /// <summary>
        ///     任务ID
        /// </summary>
        private int id;

        /// <summary>
        ///     执行时间
        /// </summary>
        private long time;

        public TimerTaskModel(int id, long time, TimerEvent execute)
        {
            this.id = id;
            this.time = time;
            this.execute = execute;
        }

        public void Run()
        {
            execute();
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public long Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}