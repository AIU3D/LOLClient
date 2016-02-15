#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			TargetSkill.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-29 16:38:41Z
//
// 描述(Description):			TargetSkill				
//
// **********************************************************************

#endregion

using GameProtocol;
using GameProtocol.Dto.FightDto;
using UnityEngine;

namespace Assets.Script.Fight.Skill
{
    public class TargetSkill:MonoBehaviour
    {
        Transform target;
        int skill;
        int userId;

        public void Init(Transform target,int skill,int userID)
        {
            this.skill = skill;
            this.target = target;
            this.userId = userID;
        }

        void Update()
        {
            if(target != null)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, 0.5f);
                if(Vector3.Distance(transform.position,target.position)< 0.1f)
                {
                    //像服务器发送伤害目标
                    DamageDTO dto = new DamageDTO();
                    dto.UserID = userId;
                    dto.Skill = skill;
                    dto.Targets = new int[][] { new int[] { target.GetComponent<PlayerCtr>().data.Id } };
                    this.Wirte(Protocol.TYPE_FIGHT,0,FightProtocol.DAMAGE_CREQ,dto);
                    Destroy(gameObject);
                }
            }
        }
    }
}