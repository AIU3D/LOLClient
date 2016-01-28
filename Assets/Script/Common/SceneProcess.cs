#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			SceneProcess.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-28 15:04:52Z
//
// 描述(Description):			SceneProcess场景界面的处理，监听一些列的UI事件				
//
// **********************************************************************

#endregion

using Assets.Script.Fight;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Script.Common
{
    public class SceneProcess:MonoBehaviour,IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.pointerId)
            {
                case PointerInputModule.kMouseLeftId:
                    //左键点击 自行扩展
                    break;
                case PointerInputModule.kMouseRightId:
                    FightScene.Instance.RightClick(eventData.position);
                    break;
            }
        }

        void Update()
        {
            Vector3 pos = Input.mousePosition;
            if(pos.x<10)
            {
                //通知相机向左移动
                FightScene.Instance.CameraHMove(1);
            }
            else if(pos.x >Screen.width - 10)
            {
                //通知相机向右移动
                FightScene.Instance.CameraHMove(-1);
            }
            else
            {
                FightScene.Instance.CameraHMove(0);
            }

            if (pos.y < 10)
            {
                //通知相机向下移动
                FightScene.Instance.CameraVMove(-1);
            }
            else if (pos.y > Screen.height - 10)
            {
                //通知相机向上移动
                FightScene.Instance.CameraVMove(1);
            }
            else
            {
                FightScene.Instance.CameraVMove(0);
            }

            if(Input.GetKey(KeyCode.Space))
            {
                FightScene.Instance.LookAt();
            }
        }
    }
}