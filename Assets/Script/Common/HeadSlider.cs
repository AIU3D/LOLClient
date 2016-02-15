#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			HeadSlider.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-28 14:29:53Z
//
// 描述(Description):			HeadSlider血条预设				
//
// **********************************************************************

#endregion

using GameProtocol.Dto.FightDto;
using UnityEngine;

namespace Assets.Script.Common
{
    public class HeadSlider:MonoBehaviour
    {
        [SerializeField]
        private SpriteSlider hp;
        [SerializeField]
        private TextMesh name;
        [SerializeField]
        private SpriteRenderer sr;
        public void Init(FightPlayerModel model,bool isFriend)
        {
            hp.Value = model.Hp / (float)model.HpMax;
            name.text = model.Name;
            if(isFriend)
            {
                sr.color = new Color(255, 255, 255, 100);
            }
        }

        public void ChangeHp(float value)
        {
            hp.Value = value;
        }

        void Update()
        {
            if(transform.rotation != Camera.main.transform.rotation)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
        }
    }
}