using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 此脚本结合ResourceManager使用 , resource下文件变动都需要点击Tools/Resources/Generate ResourcesConfig File更新resources配置表信息
/// </summary>
public class GenerateResConfig : Editor
{
    [MenuItem("/Tools/Resources/Generate ResourcesConfig File")]
    public static void Generate()
    {
        //生成资源配置文件
        //1查找Resources 目录下所有预制体文件 完整路径
        string[] resFiles =  AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/Resources"}); //读出的路径是GUID格式
        for (int i = 0; i < resFiles.Length; i++)
        {
            //2生成对应关系 名字=路径
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);//把GUID转换成路径
            //格式：Assets/Resources/UI/skill_joys.prefab
            string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);//用微软自带的方法读取不带后缀的文件名
            string filePath = resFiles[i].Replace("Assets/Resources/",string.Empty).Replace(".prefab",string.Empty);
            resFiles[i] = fileName + "=" + filePath;
            //Debug.Log(resFiles[i]);
            //3写入文件
            File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt",resFiles);
            //编译器为了性能，不会持续更新文件夹，脚本更新
            AssetDatabase.Refresh();
        }
        
    }
}
