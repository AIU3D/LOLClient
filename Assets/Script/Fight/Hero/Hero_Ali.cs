#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			Hero_Ali.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-27 16:46:40Z
//
// 描述(Description):			Hero_Ali 阿狸脚本				
//
// **********************************************************************

#endregion

#region

using System.Collections.Generic;
using Assets.Script.Fight.Skill;
using UnityEngine;

#endregion

namespace Assets.Script.Fight.Hero
{
    public class Hero_Ali : PlayerCtr
    {
        private Transform[] list;
        [SerializeField]
        private GameObject effect; // 攻击粒子
        public override void Attack(Transform[] target)
        {
            this.list = target;
            if(state == AnimState.RUN)
            {
                agent.CompleteOffMeshLink(); // 寻路结束

            }

            transform.LookAt(target[0]);
            state = AnimState.ATTACK;
            anim.SetInteger("state",AnimState.ATTACK);

        }

        public override void Attacked()
        {
            for (int i = 0; i < list.Length; i++)
            {
                GameObject go = (GameObject)Instantiate(effect, transform.position + transform.up * 2, transform.rotation);
                //让粒子向敌人位移
                go.GetComponent<TargetSkill>().Init(list[0], -1, data.Id);
                state = AnimState.IDLE;
                anim.SetInteger("state",AnimState.IDLE);
            }
        }
    }
}