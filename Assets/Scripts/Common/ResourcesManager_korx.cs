using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 资源管理器 代替Resources
    /// </summary>
    public class ResourcesManager_korx
    {
        private static Dictionary<string, string> configMap;
        //初始化类的静态数据成员
        //时机：类被加载是执行一次
        static ResourcesManager_korx()
        {
            //加载文件
            string fileContent = GetConfigFile();
            
            //解析文件 string--》Dictionary<string,string>
            BuildMap(fileContent);

        }
        /// <summary>
        /// 读取本地文件  唯一的方式
        /// </summary>
        /// <returns></returns>
        public static string GetConfigFile()//加载文件
        {
            //
            string url = "";
            //在编辑器下读取
#if UNITY_EDITOR|| UNITY_STANDALONE
            url = "file://" + Application.dataPath + "/StreamingAssets/ConfigMap.txt";
            //url =  Application.streamingAssetsPath + "/ConfigMap.txt";

#elif UNITY_IOS  //在iphone端读取
            url = "file://" + Application.dataPath + "/Raw/ConfigMap.txt";
            
#elif UNITY_ANDROID  //在android端读取
            url = "jar:file://" + Application.dataPath + "!/assets/ConfigMap.txt";
#endif

            WWW www = new WWW(url);
            while (true)
            {
                if (www.isDone)
                {
                    return www.text;
                }
            }


        }

        private static void BuildMap(string fileContent)//解析文件 string--》Dictionary<string,string>
        {
            configMap = new Dictionary<string, string>();
            // 格式 ： 文件名=路径\r\n文件名=路径
            using (StringReader reader = new StringReader(fileContent))
            {
                string line;
                while ((line = reader.ReadLine()) !=null)
                {
                    string[] keyvalue = line.Split('=');
                    configMap.Add(keyvalue[0],keyvalue[1]);
                }
                //while (line != null)
                //{
                //    string[] keyValue = line.Split('=');
                //    //keyValue[0]文件名 keyValue[1]路径
                //    configMap.Add(keyValue[0], keyValue[1]);
                //    line = reader.ReadLine();
                //}
            }//当程序退出using代码块，将自动调用reader.Dispose()方法
        }


        /// <summary>
        /// 名称变路径
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public static T Load<T>(string prefabName) where T : Object
        {
            //判断字典中有没有
            if (!configMap.ContainsKey(prefabName))
            {
                Debug.LogError("Resorces配置表未包含需要添加的预制体  运行前请点击Tools/Resources/Generate*******刷新Resources配置表");
                return null;
            }
            string prefabPath = configMap[prefabName];
            T t = Resources.Load<T>(prefabPath);
            if (t)
                return t;
            else
            {
                Debug.LogError("未查询到有"+prefabName +"名字的预制体, 检查预制体名字,或者实例化预制体的字段是否和预制体名字是否一致");
                return null;

            }
        }

    }

}