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

using GameProtocol.Dto.FightDto;
using UnityEngine;

namespace Assets.Script.Fight
{
    public class PlayerCtr:MonoBehaviour
    {
        /// <summary>
        /// 角色数据
        /// </summary>
        public FightPlayerModel data;

        /// <summary>
        /// 动画控制器
        /// </summary>
        protected Animator anim;

        private NavMeshAgent agent;

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
        }

        /// <summary>
        /// 申请攻击
        /// </summary>
        public void Attack(Transform[] target)
        {
            
        }

        /// <summary>
        /// 攻击结束回调方法
        /// </summary>
        public void Attacked()
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
    }
}