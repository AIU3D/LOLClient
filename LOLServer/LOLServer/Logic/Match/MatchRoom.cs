#region

using System.Collections.Generic;

#endregion

namespace LOLServer.Logic.Match
{
    /// <summary>
    ///     战斗匹配房间模型
    /// </summary>
    public class MatchRoom
    {
        private int teamMax = 1;
        private List<int> teamOne = new List<int>();
        private List<int> teamTwo = new List<int>();
        public int Id { get; set; }

        public int TeamMax
        {
            get { return teamMax; }
            set { teamMax = value; }
        }

        public List<int> TeamOne
        {
            get { return teamOne; }
            set { teamOne = value; }
        }

        public List<int> TeamTwo
        {
            get { return teamTwo; }
            set { teamTwo = value; }
        }
    }
}