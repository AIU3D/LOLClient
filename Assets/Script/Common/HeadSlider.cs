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

        public void Init(FightPlayerModel model)
        {
            hp.Value = model.Hp / (float)model.HpMax;
            name.text = model.Name;
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