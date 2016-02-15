using System.Collections.Generic;
using LOLServer.DAO.Model;
using NetFrame;

namespace LOLServer.Cache.impl
{
    public class AccountCache:IAccountCache
    {
        public int index = 0;

        /// <summary> 连接对象与账号之间的映射绑定 判断是否在线</summary>
        private Dictionary<UserToken, string> onlineAccMap = new Dictionary<UserToken, string>();

        /// <summary> 账号与自身具体属性的映射绑定</summary>
        private Dictionary<string, AccountModel> accMap = new Dictionary<string, AccountModel>(); 
        public bool HasAccount(string account)
        {
            return accMap.ContainsKey(account);
        }

        public bool Match(string account, string password)
        {
            if (!HasAccount(account)) return false;
            
            //获取账号信息，判断密码是否匹配
            return accMap[account].Password.Equals(password);
        }

        public bool IsOnline(string account)
        {
            //判断当前是否有此账号，没有则说明不在线
            return onlineAccMap.ContainsValue(account);
        }

        public int GetID(NetFrame.UserToken token)
        {
            //玩家不在线则返回-1
            if (!onlineAccMap.ContainsKey(token)) return -1;
            
            //返回账号ID
            return accMap[onlineAccMap[token]].ID;
        }

        public void Online(NetFrame.UserToken token, string account)
        {
            //在事务层已经判断，所以直接添加账号映射
            onlineAccMap.Add(token, account);
        }

        public void Offline(NetFrame.UserToken token)
        {
            //如果当前连接登录，则移除
            if (onlineAccMap.ContainsKey(token)) onlineAccMap.Remove(token);
        }

        public void Add(string account, string password)
        {
            AccountModel model = new AccountModel();
            model.Account = account;
            model.Password = password;
            model.ID = index;
            accMap.Add(account,model);

            index++;
        }
    }
}