using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

//public interface IResetable
//{
//    void OnReset();
//}
namespace Common
{
    /// <summary>
    /// 对象池  包含3d和ui
    /// </summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        private Dictionary<string, List<GameObject>> cache;

        public Dictionary<string, List<GameObject>> cachePublic
        {
            get
            {
                return cache;
            }
        }
        public override void Init() //单例中创建时初始化
        {
            base.Init();
            cache = new Dictionary<string, List<GameObject>>();
        }
       //3D物体对象池
       //从对象池取出
        public GameObject CreateObject(string key, GameObject prefab,Transform parent, Vector3 pos, Quaternion rotate )
        {
            GameObject go = null;
            //1查找
            go = FindUsableObject(key);
            //2 go为空则 添加
            if (go == null)  //go为null说明没有物体active为false
            {
                go = AddObject(key, prefab , parent); //没有物体，创建物体
            }
            //3使用
            UseObject(go ,pos, rotate, parent);

            return go;
        }
        public GameObject CreateObject_local(string key, GameObject prefab, Vector3 pos, Quaternion rotate ,Vector3 scale) //,Transform parent
        {
            GameObject go = null;
            //1查找
            go = FindUsableObject(key);
            //2 go为空则 添加
            if (go == null)  //go为null说明没有物体active为false
            {
                go = AddObject(key, prefab ,null); //没有物体，创建物体
            }
            //3使用
            UseObject_local(go ,pos, rotate , scale);

            return go;
        }
        //返回对象池
        public void CollectObject(GameObject go)
        {
            if (go != null)
                go.SetActive(false);
        }
        public void CollectObject(GameObject go, float delay)
        {
            StartCoroutine(CollectObject_Delay(go, delay));
        }
        private IEnumerator CollectObject_Delay(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (go != null)
                go.SetActive(false);
        }
        private void UseObject(GameObject go , Vector3 pos, Quaternion rotate,Transform parent)
        {
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.transform.SetParent(parent);
            go.SetActive(true);

            //go.GetComponent<IResetable>().OnReset(); //接口使用
        }
        private void UseObject_local(GameObject go , Vector3 pos, Quaternion rotate , Vector3 scale)
        {
            go.transform.localPosition = pos;
            go.transform.localRotation = rotate;
            go.transform.localScale = scale;

            go.SetActive(true);

            //go.GetComponent<IResetable>().OnReset(); //接口使用
        }
        private GameObject AddObject(string key, GameObject prefab,Transform parent)
        {
            if (prefab == null)
            {
                Debug.Log("预制体为空,无法实例化");
            }
            GameObject go = Instantiate(prefab,parent , true);
            if (!cache.ContainsKey(key))
                cache.Add(key, new List<GameObject>());
            cache[key].Add(go);
            return go;
        }

        public GameObject FindUsableObject(string key)
        {
            if (cache.ContainsKey(key)) //有风险，先判断是否有这个键
            {
                checkGoFromKey(key);
                return cache[key].Find(g => !g.activeSelf); //通过key查找字典里是否含有active为false的物体

            }
            return null;
        }
        //清空(删除)指定key对用的物体
        public void Clear(string key)
        {
            for (int i = 0; i < cache[key].Count; i++)
            {
                Destroy(cache[key][i].gameObject);
            }

            cache.Remove(key);
        }
        //清空所有对象池
        public void ClearAll()
        {
            List<string> keylist = new List<string>(cache.Keys);
            foreach (var key in keylist)  //直接遍历字典会报错
            {
                Clear(key);
            }
        }

        
        
        //ui对象池使用方法
        //public void GeneratePromptingPanel(Vector2 pos, Transform parent)
        //{
        //    GameObject promptingPanel = ResourcesManager_korx.Load<GameObject>("PromptingPanel");
        //    GameObject go = GameObjectPool.Instance.CreateUIObject("PromptingPanel", promptingPanel, pos, parent);
        //    GameObjectPool.Instance.CollectObject(go, 1.5f);

        //}
        //额外的功能
        //1.通过key返回对象池所有物体
        public GameObject[] FindObjectFromKey(string key)
        {
            if (!cache.ContainsKey(key))
                return null;
            return cache[key].ToArray();

        }

        //检查对象池中有没有被删掉的物体  移除他
        public void checkGoFromKey(string key)
        {
            if (cache.ContainsKey(key))
            {
                for (int i = cache[key].Count -1; i >= 0; i--)
                {
                    if (cache[key][i].gameObject == null)
                    {
                        cache[key].Remove(cache[key][i]);
                    }
                }
            }
            
        }
        
       
        //根据传进来的key回收对象池
        public void CollectObjects_byKey(string key)
        {
            if (cache.ContainsKey(key))
            {
                foreach (var go in cache[key])
                {
                    CollectObject(go);
                }
            }
        }
        //根据传进来的keys回收对象池
        public void CollectObject_byKeys(List<string> keys)
        {
            foreach (var key in keys)  //直接遍历字典会报错
            {
                CollectObjects_byKey(key);
            }
        }
        
        //回收所有对象池
        public void CollectAllObject()
        {
            foreach (var key in cache.Keys)  //直接遍历字典会报错
            {
                foreach (var go in cache[key])  //直接遍历字典会报错
                {
                    CollectObject(go);
                }
            }
        }

        //通过key判断对象池中是否有显示的物体
        public bool FindActiveObject(string key)
        {
            if (cache.ContainsKey(key)) //有风险，先判断是否有这个键
            {
                checkGoFromKey(key);
                return cache[key].Find(g => g.activeSelf); //通过key查找字典里是否含有active为false的物体

            }
            return false;
        }
        
        
    }
}
