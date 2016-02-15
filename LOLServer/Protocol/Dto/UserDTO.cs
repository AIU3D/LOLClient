using System;

namespace GameProtocol.Dto
{
    [Serializable]
    public class UserDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int RunCount { get; set; }
        public int[] HeroList { get; set; }
        public UserDTO() { }

        public UserDTO(int id, string name, int level, int exp, int winCount, int loseCount, int runCount,int[] heroList)
        {
            ID = id;
            Name = name;
            Level = level;
            Exp = exp;
            WinCount = winCount;
            LoseCount = loseCount;
            RunCount = runCount;
            HeroList = heroList;
        }
    }
}