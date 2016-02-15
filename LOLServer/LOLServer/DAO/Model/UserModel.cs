using System.Collections.Generic;

namespace LOLServer.DAO.Model
{
    /// <summary>
    ///     角色信息模型
    /// </summary>
    public class UserModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int RunCount { get; set; }

        public List<int> HeroList { get; set; }
        public int AccountID { get; set; }
        public UserModel()
        {
            this.Level = 0;
            this.Exp = 0;
            this.WinCount = 0;
            this.LoseCount = 0;
            this.RunCount = 0;
        }
    }
}