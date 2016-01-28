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
            HeadIcon.sprite = Resources.Load<Sprite>("HeroIcon/" + model.Code);
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

        private int cameraH;
        private int cameraV;

        public float CameraSpeed = 1.0f;
        /// <summary>
        /// 相机横移 1 -1 表示方向 1表示左 -1表示右
        /// </summary>
        public void CameraHMove(int dir)
        {
            if (cameraH != dir)
                cameraH = dir;
        }

        /// <summary>
        /// 相机纵移 1表示上 -1 表示下
        /// </summary>
        public void CameraVMove(int dir)
        {
            if (cameraV != dir)
                cameraV = dir;
        }

        /// <summary>
        /// 对相机的位移  z最大150 最小0 x最小为40 最大160
        /// </summary>
        private void Update()
        {
            float x = mainCamera.transform.position.x;
            float y = mainCamera.transform.position.y;
            float z = mainCamera.transform.position.z;
            switch (cameraH)
            {
                case 1:
                    if(mainCamera.transform.position.z < 150)
                    {
                        mainCamera.transform.position = new Vector3(x, y, z + cameraH);
                    }
                    break;
                case -1:
                    if (mainCamera.transform.position.z >0)
                    {
                        mainCamera.transform.position = new Vector3(x, y, z + cameraH);
                    }
                    break;
            }

            switch (cameraV)
            {
                case 1:
                    if (mainCamera.transform.position.x < 160)
                    {
                        mainCamera.transform.position = new Vector3(x + cameraV, y, z);
                    }
                    break;
                case -1:
                    if (mainCamera.transform.position.x > 40)
                    {
                        mainCamera.transform.position = new Vector3(x + cameraV, y, z);
                    }
                    break;
            }
        }
    
        public void RightClick(Vector3 pos)
        {
            Ray ray = mainCamera.ScreenPointToRay(pos);

            RaycastHit[] hits = Physics.RaycastAll(ray, 200);

            for (int i = 0; i < hits.Length; i++)
            {
                //如果是地方单位进行普通攻击
                //己方单位无视
                //如果是地板层 则开始寻路
                if(hits[i].transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                {
                    MoveDTO dto = new MoveDTO();
                    dto.X = hits[i].point.x;
                    dto.Y = hits[i].point.y;
                    dto.Z = hits[i].point.z;
                    this.Wirte(Protocol.TYPE_FIGHT,0,FightProtocol.MOVE_CREQ,dto);
                    return;
                }
            }
        }
    
    }
}