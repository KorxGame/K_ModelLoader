using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common
{
    public static class TransformHelper
    {
        #region 通过名字查找物体

        //调用 FindChildByName_base 主要为了打印日志
        public static Transform FindChildByName(this Transform childTF, string childName)
        {
            if (childTF == null || string.IsNullOrEmpty(childName)) //childName为"" 空字符串时 返回的是childTF , 直接返回null
            {
                Debug.LogWarning("TransformHelper: 查找原节点空");
                return null;
            }

            //Debug.Log("查找");
            Transform temp = childTF.FindChildByName_base(childName);
            if (temp == null)
            {
                Debug.LogWarning("未找到名为" + childName + "的节点");
            }

            return temp;
        }

        //同上 可以在查找到后 设置显隐
        public static Transform FindChildByName(this Transform childTF, string childName, bool isActive)
        {
            if (childTF == null || string.IsNullOrEmpty(childName)) //childName为"" 空字符串时 返回的是childTF , 直接返回null
            {
                Debug.LogWarning("TransformHelper: 查找原节点空");
                return null;
            }

            //Debug.Log("查找");
            Transform temp = childTF.FindChildByName_base(childName);
            if (temp == null)
            {
                Debug.Log("未找到名为" + childName);
            }
            else
            {
                temp.SetActive(isActive);
            }

            return temp;
        }

        //递归查找子物体下目标名字的物体名字
        private static Transform FindChildByName_base(this Transform childTF, string childName)
        {
            //Debug.Log("查找");
            Transform temp = childTF.Find(childName);
            if (temp != null) return temp;

            for (int i = 0; i < childTF.childCount; i++)
            {
                temp = FindChildByName_base(childTF.GetChild(i), childName);
                if (temp != null) return temp;
            }

            return null;
        }

        #endregion


        //删除目标下面所有物体
        public static void DestroyAllChild(this Transform trans)
        {
            for (int i = trans.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(trans.GetChild(i).gameObject);
                Debug.Log("已删除");
            }
        }
        //设置显隐 SetActive 和

        public static void SetActive(this Transform trans, bool isActive)
        {
            if (trans != null)
            {
                if (trans.gameObject.activeSelf != isActive)
                    trans.gameObject.SetActive(isActive);
            }
            else
            {
                Debug.Log("SetActive:该transform为空:");
            }
        }


        #region 组件

        /// <summary>
        /// 查找是否有目标组件,没有的话动态添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="childTF"></param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this Transform childTF) where T : Component
        {
            if (childTF.GetComponent<T>() == null)
                childTF.gameObject.AddComponent<T>();
            return childTF.GetComponent<T>();
        }

        public static T GetOrAddComponentInchild<T>(this Transform childTF, string childName) where T : Component
        {
            Transform child = FindChildByName(childTF, childName);
            if (!child) return null;
            child.GetOrAddComponent<T>();
            return child.GetComponent<T>();
        }

        //对子物体查找某个组件
        public static T GetComponentByChildName<T>(this Transform childTF, string childName) where T : Component
        {
            T t;
            Transform child = FindChildByName(childTF, childName);
            if (!child) return null;
            if (child.TryGetComponent(out t))
            {
                return t;
            }
            else
            {
                Debug.LogError("未找到目标组件");
                return null;
            }
        }

//查找子物体上的组件 并返回  不包含孙物体
        //对子物体查找某个组件
        public static T[] GetComponentsInOnlyChild<T>(this Transform childTF) where T : Component
        {
            List<T> list = new List<T>();
            for (int i = 0; i < childTF.childCount; i++)
            {
                T t;
                if (childTF.GetChild(i).TryGetComponent(out t))
                {
                    list.Add(t);
                }
            }

            return list.ToArray();
        }

        //通过名字查找子物体,并在子物体下查找某组件
        public static T GetComponentInChildByChildName<T>(this Transform childTF, string name) where T : Component
        {
            T t;
            Transform child = childTF.FindChildByName(name);
            if (child != null)
            {
                t = child.GetComponentInChildren<T>();
                if (t != null)
                    return t;
            }
            return null;
            
        }
        
        //获取子物体下的组件 不包含 被查找的物体本身 , 且除了exceptName名字的子物体下
        public static T[] GetComponentsInChildExceptChildName<T>(this Transform childTF, string exceptName) where T : Component
        {
            List<T> list = new List<T>();
            for (int i = 0; i < childTF.childCount; i++)
            {
                if(childTF.GetChild(i).name.Contains(exceptName))
                    continue;
                T[] ts = childTF.GetChild(i).GetComponentsInChildren<T>();
                list.Union(ts.ToList());

            }
            return list.ToArray();
        }
        
        
        
        
        

        #endregion

        #region 路径

//通过物体查找路径
        public static string GetPath(this GameObject gameobject, string splitter = "/")
        {
            return gameobject.transform.GetPath(splitter);
        }

        public static string GetPath(this Transform transform, string splitter = "/")
        {
            if (transform == null)
            {
                Debug.Log("GetPath:Error: 目标为空无法获取");
                return null;
            }

            var result = transform.name;
            var parent = transform.parent;
            while (parent != null)
            {
                result = $"{parent.name}{splitter}{result}";
                parent = parent.parent;
            }

            return result;
        }
        //从子节点到某个父节点
        public static string GetPath(this Transform childTransform,Transform parentTrans, string splitter = "/")
        {
            if (childTransform == null || parentTrans==null)
            {
                Debug.Log("GetPath:Error: 目标为空无法获取");
            }

            var result =childTransform.name;
            var parent = childTransform.parent;
            while (parent != parentTrans)
            {
                result = $"{parent.name}{splitter}{result}";
                parent = parent.parent;
            }

            return result;
        }



        //通过路径查找物体
        public static GameObject GetGoByPath(this string Path)
        {
            GameObject go = GameObject.Find(Path);
            if (go == null)
            {
                Debug.Log("未找到此路径的物体" + Path);
            }

            return go;
        }
        public static bool GetGoByPath(this string Path , out GameObject go)
        {
            GameObject _go = GameObject.Find(Path);
            if (_go == null)
            {
                Debug.Log("未找到此路径的物体" + Path);
                go = null;
                return false;
            }
            go = _go;
            return true;
        }

        #endregion

        #region 矩阵

        public static Quaternion ExtractRotation(this Matrix4x4 matrix)
        {
            Vector3 forward;
            forward.x = matrix.m02;
            forward.y = matrix.m12;
            forward.z = matrix.m22;

            Vector3 upwards;
            upwards.x = matrix.m01;
            upwards.y = matrix.m11;
            upwards.z = matrix.m21;

            return Quaternion.LookRotation(forward, upwards);
        }

        public static Vector3 ExtractPosition(this Matrix4x4 matrix)
        {
            Vector3 position;
            position.x = matrix.m03;
            position.y = matrix.m13;
            position.z = matrix.m23;
            return position;
        }

        public static Vector3 ExtractScale(this Matrix4x4 matrix)
        {
            Vector3 scale;
            scale.x = new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude;
            scale.y = new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude;
            scale.z = new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude;
            return scale;
        }

        #endregion


        //public static void RemoveComponent<T>(this Transform trans)
        //{
        //    T t;
        //    if(trans.TryGetComponent(out t))
        //    {
        //        GameObject.Destroy(t);
        //    }
        //}
    }
}