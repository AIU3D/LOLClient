#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			WarFog.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-26 11:25:29Z
//
// 描述(Description):			WarFog战争迷雾				
//
// **********************************************************************

#endregion

using UnityEngine;

namespace Assets.WarFog
{
    public class WarFog:MonoBehaviour
    {

        public RenderTexture mask;
        public Material mat;

        public void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            mat.SetTexture("_MaskTex", mask);
            Graphics.Blit(source, destination, mat);
        }

    }
}