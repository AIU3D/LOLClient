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

using System.Runtime.InteropServices;
using Assets.Script.Common;
using GameProtocol;
using GameProtocol.Constans;
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
        [SerializeField]
        private Slider HpSlider;
        [SerializeField]
        private Text Name;
        [SerializeField]
        private Text Level;
        [SerializeField]
        private SkillGrid[] skills;

        private Camera mainCamera;
        private GameObject hero;
        private void Start()
        {
            mainCamera = Camera.main;
            Instance = this;
            this.Wirte(Protocol.TYPE_FIGHT, 0, FightProtocol.ENTER_CREQ, null);
        }

        /// <summary>
        /// 初始化显示
        /// </summary>
        /// <param name="model"></param>
        public void InitView(FightPlayerModel model,GameObject hero)
        {
            this.hero = hero;
            HeadIcon.sprite = Resources.Load<Sprite>("HeadIcon/" + model.Code);
            HpSlider.value = model.Hp / (float)model.HpMax;
            Name.text = HeroData.heroMap[model.Code].Name;
            Level.text = model.Level.ToString();

            for (int i = 0; i < model.Skills.Length; i++)
            {
                skills[i].Init(model.Skills[i]);
            }
        }

        public void LookAt()
        {
            this.mainCamera.transform.position = hero.transform.position + new Vector3(-6, 8, 0);
        }
    }
}