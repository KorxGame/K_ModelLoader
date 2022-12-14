using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject("Singleton of" + typeof(T)).AddComponent<T>();

                    }
                    else
                    {
                        _instance.Init();
                    }
                }

                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                Init();
            }
        }

        //MonoSingleton实例化是会调用 init
        public virtual void Init()
        {

        }
    }
}