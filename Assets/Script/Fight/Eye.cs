#region 描述

// **********************************************************************
// 
// 文件名(File Name)：			Eye.cs
// 
// 作者(Author)：				da_fei
// 
// 创建时间(CreateTime):			2016-01-29 10:39:04Z
//
// 描述(Description):			Eye 视野脚本 挂载在地方单位上		
//
// **********************************************************************

#endregion

#region

using System.Collections.Generic;
using UnityEngine;

#endregion

namespace Assets.Script.Fight
{
    public class Eye : MonoBehaviour
    {
        [SerializeField]
        private GameObject head;
        private List<GameObject> list = new List<GameObject>();
       [SerializeField]
        private GameObject root;
        [SerializeField]
        private GameObject hp;

        public void OnTriggerEnter(Collider other)
        {
            PlayerCtr ctr = other.gameObject.GetComponent<PlayerCtr>();
            if (ctr != null)
            {
                if (ctr.data.Team != GetComponent<PlayerCtr>().data.Team)
                {
                    list.Add(other.gameObject);
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (list.Contains(other.gameObject))
            {
                list.Remove(other.gameObject);
            }
        }

        private void Update()
        {
            if (list.Count > 0)
            {
                //是否隐身
                //敌方是否反隐
                if(!head.activeSelf)
                {
                    head.SetActive(true);
                }
                if(!hp.activeSelf)
                {
                    hp.SetActive(true);
                }
                if(!root.activeSelf)
                {
                    root.SetActive(true);
                }
            }
            else
            {
                if (head.activeSelf)
                {
                    head.SetActive(true);
                }
                if (hp.activeSelf)
                {
                    hp.SetActive(true);
                }
                if (root.activeSelf)
                {
                    root.SetActive(true);
                }
            }
        }
    }
}