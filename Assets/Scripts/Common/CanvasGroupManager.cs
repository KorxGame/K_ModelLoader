using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public static class CanvasGroupManager
    {
        
        /// <summary>
        /// 传入transform  自动获取添加canvasGroup
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="active"></param>
        /// <param name="blocksRaycasts">面板是否能被射线检测到</param>
        public static void SetActive_cg(this Transform trans ,bool active ,bool blocksRaycasts)
        {
            CanvasGroup cg = trans.GetOrAddComponent<CanvasGroup>();
            SetActive_cg(cg,active,blocksRaycasts);
        }
        /// <summary>
        /// 设置CanvasGroup显示隐藏
        /// </summary>
        /// <param name="canvasGroup"> 传入目标canvasGroup</param>
        /// <param name="active"> 是否显示隐藏 </param>
        /// <param name="blocksRaycasts"> 是否开启射线检测  true:根据 active开启关闭  ,false:一直关闭   默认随着active开启关闭 </param>
        public static void SetActive_cg(this CanvasGroup canvasGroup , bool active ,bool blocksRaycasts = true)
        {
            if (!canvasGroup.gameObject.activeInHierarchy)
            {
                canvasGroup.gameObject.SetActive(true);
            }
            
            canvasGroup.alpha = active ? 1 : 0;
            canvasGroup.blocksRaycasts = blocksRaycasts ? active:false;
        }
    
   
        //判断canvasgroup状态
        public static bool GetActive_cg(this CanvasGroup canvasGroup)
        {
            return canvasGroup.alpha == 1;
        }
        
        
        
        
        
        
        

    }

}

