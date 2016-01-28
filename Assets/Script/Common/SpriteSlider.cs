#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			SpriteSlider.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-27 18:35:27Z
//
// 描述(Description):			SpriteSlider英雄3D血条制作				
//
// **********************************************************************

#endregion

using UnityEngine;

namespace Assets.Script.Common
{
    public class SpriteSlider:MonoBehaviour
    {
        [SerializeField]
        private Transform front;
        private float m_value;
        public float Value
        {
            get { return m_value;}
            set
            {
                m_value = value;
                front.localScale = new Vector3(value, 1, 1);
                front.localPosition = new Vector3(-1 * (1 - value) * 0.8f, 0);
            }
        }
    }
}