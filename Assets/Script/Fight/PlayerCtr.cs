#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			PlayerCtr.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-27 16:49:20Z
//
// 描述(Description):			PlayerCtr 角色控制类基类				
//
// **********************************************************************

#endregion

using Assets.Script.Common;
using GameProtocol.Dto.FightDto;
using UnityEngine;

namespace Assets.Script.Fight
{
    public class PlayerCtr:MonoBehaviour
    {
        /// <summary>
        /// 角色数据
        /// </summary>
       [HideInInspector]
        public FightPlayerModel data;

        /// <summary>
        /// 动画控制器
        /// </summary>
        protected Animator anim;

        protected  NavMeshAgent agent;
        [SerializeField]
        private GameObject mask; //战争迷雾剔除面板
        [SerializeField]
        private HeadSlider headSlider; // 3D血条
        [SerializeField]
        private MeshRenderer head;

        protected int state = AnimState.IDLE;
        void Start()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="target">目标位置</param>
        public void Move(Vector3 target)
        {
            //重置寻路
            agent.ResetPath();
            //设置目标点
            agent.SetDestination(target);
            anim.SetInteger("state",AnimState.RUN);
            state = AnimState.RUN;
        }

        /// <summary>
        /// 申请攻击
        /// </summary>
        public virtual void Attack(Transform[] target)
        {
            
        }

        /// <summary>
        /// 攻击结束回调方法
        /// </summary>
        public virtual void Attacked()
        {

        }

        /// <summary>
        /// 申请技能
        /// </summary>
        /// <param name="code"></param>
        public void Skill(int code,GameObject[] target)
        {
            
        }

        /// <summary>
        /// 技能回调方法
        /// </summary>
        public void Skilled()
        {
            
        }

        /// <summary>
        /// 遮罩的显示与隐藏
        /// </summary>
        /// <param name="state"></param>
        private void MaskState(bool state)
        {
            mask.SetActive(state);
        }

        public void Init(FightPlayerModel data,int myTeam)
        {
            bool isFriend = data.Team == myTeam;
            this.data = data;
            headSlider.Init(data, isFriend);
            head.material.SetTexture("_MainTex",Resources.Load<Texture>("HeroIcon/" + data.Code));
            //如果不是己方单位，移出遮罩
            //是己方单位，移出视野脚本
            if (!isFriend)
            {
                gameObject.layer = LayerMask.NameToLayer("Enemy");
                mask.SetActive(false);
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Friend");
                Destroy(GetComponent<Eye>());
            }
        }

        private void Update()
        {
            switch (state)
            {
                case AnimState.RUN:
                    if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0 && !agent.pathPending) //pathPending 正在计算路径，但还没有准备好
                    {
                        state = AnimState.IDLE;
                        anim.SetInteger("state",AnimState.IDLE);
                    }
                    else
                    {
                        //代理目前定位上OffMeshLink。
                        if(agent.isOnOffMeshLink)
                        {
                            //终止电流OffMeshLink。
                            agent.CompleteOffMeshLink();
                        }
                    }
                    break;
            }
        }

        public void HpChange()
        {
            headSlider.ChangeHp(1f * data.Hp / data.HpMax);
        }
    }
}