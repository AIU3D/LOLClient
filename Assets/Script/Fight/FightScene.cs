#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			FightScene.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-27 11:28:44Z
//
// 描述(Description):			FightScene战斗场景管理				
//
// **********************************************************************

#endregion

#region

using GameProtocol;
using GameProtocol.Dto.FightDto;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Assets.Script.Fight
{
    public class FightScene : MonoBehaviour
    {

        public static FightScene Instance;

        [SerializeField]
        private Image HeadIcon;
        private void Start()
        {
            Instance = this;
            this.Wirte(Protocol.TYPE_FIGHT, 0, FightProtocol.ENTER_CREQ, null);
        }

        public void RefereshView(FightPlayerModel model)
        {
            
        }
    }
}