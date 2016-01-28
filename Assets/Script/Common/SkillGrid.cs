#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			SillGrid.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-28 10:17:00Z
//
// 描述(Description):			SillGrid 技能单元格				
//
// **********************************************************************

#endregion

using GameProtocol.Dto.FightDto;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Common
{
    public class SkillGrid:MonoBehaviour
    {
      
        private Image mask;
        
        private Image background;

        private FightSkill skill;

        private bool sclied = false;

        private float maxTime = 0;
        private float nowTime = 0;

        [SerializeField]
        private Button LevelUpButton;
        /// <summary>
        /// 技能初始化
        /// </summary>
        /// <param name="skill"></param>
        public void Init(FightSkill skill)
        {
            this.skill = skill;
            Sprite sp = Resources.Load<Sprite>("SkillIcon/" + skill.Code);
            background.sprite = sp;
            mask.gameObject.SetActive(true);
        }

        /// <summary>
        /// 设置遮罩
        /// </summary>
        /// <param name="time"></param>
        public void SetMask(float time)
        {
            //如果为0 说明要取消冷却显示
            if (time == 0)
            {
                //当前不是冷却状态，并且技能等级大于0 时 取消遮罩
                if (!sclied && skill.SkillLevel > 0)
                {
                    mask.gameObject.SetActive(false);
                }
                else
                {
                    mask.gameObject.SetActive(true);
                    return;
                }
            }

            maxTime = time;
            nowTime = time;
            mask.gameObject.SetActive(true);
            sclied = true;
            
        }

        void Awake()
        {
            mask = this.GetComponentInChildren<Image>();
            background = this.GetComponent<Image>();
        }

        void Update()
        {
            if (!sclied) return;
            nowTime -= Time.deltaTime;
            if(nowTime <= 0)
            {
                nowTime = 0;
                sclied = false;
                mask.gameObject.SetActive(false);
            }

            mask.fillAmount = nowTime / maxTime;
        }

        /// <summary>
        /// 鼠标进入事件
        /// </summary>
        public void PointEnter()
        {
            //显示Tip
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        public void PointExit()
        {
            //关闭Tip
        }

        /// <summary>
        /// 按钮是否可以点击
        /// </summary>
        /// <param name="state"></param>
        public void SetBtnState(bool state)
        {
            LevelUpButton.interactable = false;
        }

        public void LevelUp()
        {
            //向服务器发送消息申请升级技能
        }
    }
}