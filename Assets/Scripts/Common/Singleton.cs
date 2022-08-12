using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Common
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }
        }

        protected static void SetInstance(T newInstance)
        {
            _instance = newInstance;
        }
    }




}
