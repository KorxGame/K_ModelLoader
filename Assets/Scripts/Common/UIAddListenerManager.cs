using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public static class UIAddListenerManager
    {
        //button的添加事件
        public static void Find_AddListener(this Transform transform, string childName, Action condition)
        {
            Button btn;
            Transform trans = transform.FindChildByName(childName);
            if (trans && trans.TryGetComponent(out btn))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    condition();//?.Invoke()
                });

            }
            else
            {
                if (!trans)
                    Debug.LogError("未查找到该子物体:" + childName);
                else
                {
                    Debug.LogError(childName + "上未找到button组件");
                }

            }
        }
        //toggle事件
        public static void Find_AddListener(this Transform transform, string childName, Action<bool> condition)
        {
            Toggle toggle;
            Transform trans = transform.FindChildByName(childName);
            if (trans && trans.TryGetComponent(out toggle))
            {
                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener((bool value) =>
                {
                    //Debug.Log("事件");
                    condition?.Invoke(value);
                });

            }
            else
            {
                if (!trans)
                    Debug.LogError("未查找到该子物体:" + childName);
                else
                {
                    Debug.LogError(childName + "上未找到toggle组件");
                }
            }
        }

        //slider
        public static void Find_AddListener(this Transform transform, string childName, Action<float> condition)
        {
            Slider slider;
            Transform trans = transform.FindChildByName(childName);
            if (trans && trans.TryGetComponent(out slider))
            {
                slider.onValueChanged.AddListener((float value) =>
                {
                    condition(value);
                });

            }
            else
            {
                if (!trans)
                    Debug.LogError("未查找到该子物体:" + childName);
                else
                {
                    Debug.LogError(childName + "上未找到slider组件");
                }
            }
        }


    }

}
