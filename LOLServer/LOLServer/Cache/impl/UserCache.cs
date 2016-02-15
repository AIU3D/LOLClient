using System.Collections.Generic;
using LOLServer.DAO.Model;
using NetFrame;

namespace LOLServer.Cache.impl
{
    public class UserCache:IUserCache
    {
        /// <summary>
        /// 用户ID和模型的映射表
        /// </summary>
        private Dictionary<int, UserModel> idToModel = new Dictionary<int, UserModel>();

        /// <summary>
        /// 账号ID和角色ID的绑定
        /// </summary>
        private Dictionary<int, int> accToUid = new Dictionary<int, int>();

        
        private Dictionary<int, UserToken> idToToken = new Dictionary<int, UserToken>();

        private Dictionary<UserToken, int> tokenToId = new Dictionary<UserToken, int>();

        private int index = 0;

        public bool CreateRole(UserToken token, string name, int accountID)
        {
            UserModel user = new UserModel();
            user.Name = name;
            user.ID = index++;
            user.AccountID = accountID;
            List<int> list = new List<int>();
            for (int i = 1; i < 8; i++)
            {
                list.Add(i);
            }
            user.HeroList = list;
            //创建成功后进行账户ID和用户ID的绑定
            accToUid.Add(accountID,user.ID);
            //创建成功后进行用户ID和账户ID的绑定
            idToModel.Add(user.ID,user);
            return true;
        }

        public bool HasRole(UserToken token)
        {
            return tokenToId.ContainsKey(token);
        }

        public bool HasByAccountId(int id)
        {
            return accToUid.ContainsKey(id);
        }

        public UserModel GetUser(UserToken token)
        {
            if (!HasRole(token)) return null;
            return idToModel[tokenToId[token]];
        }

        public UserModel GetUser(int id)
        {
            return idToModel[id];
        }

        public UserModel OnLine(UserToken token, int id)
        {
            idToToken.Add(id,token);
            tokenToId.Add(token,id);
            return idToModel[id];
        }

        public void OffLine(UserToken token)
        {
            if (tokenToId.ContainsKey(token))
            {
                if (idToToken.ContainsKey(tokenToId[token]))
                {
                    idToToken.Remove(tokenToId[token]);
                }
                tokenToId.Remove(token);
            }
        }

        public UserToken GetToken(int id)
        {
            return idToToken[id];
        }

        public UserModel GetByAccountID(int id)
        {
            if (!accToUid.ContainsKey(id)) return null;
            return idToModel[accToUid[id]];
        }

        public bool IsOnLine(int id)
        {
            return idToToken.ContainsKey(id);
        }
    }
}