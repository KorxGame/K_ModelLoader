using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class LayerHelper
{
    static string[] layerNames = new string[] { "MainModel", "Disassemble" };
    [MenuItem("/Tools/添加layers")]
    public static void CreateLayer()
    {
        for (int i = 0; i < layerNames.Length; i++)
        {
            if (AutoAddLayer(layerNames[i]))
            {
                Debug.Log("添加成功:" + layerNames[i]);
            }
            else
            {
                Debug.Log("添加失败:" + layerNames[i]);
            }
        }
    }


    //改变目标节点 的layer 默认不改变子节点的
    public static void ChangeLayer(this GameObject go, string layerName, bool changeChildrenNodes = false)
    {
        LayerMask layer = LayerMask.NameToLayer(layerName);
        if (layer != -1)
        {
            go.layer = layer;
            return;
        }

        bool result = AutoAddLayer(layerName);

        Debug.Log(result);
        if (result)
        {
            layer = LayerMask.NameToLayer(layerName);
            if (!changeChildrenNodes)
                go.layer = layer;
            else
            {
                go.transform.ChangeLayer(layer);
            }
        }

    }


    static void ChangeLayer(this Transform trans, LayerMask targetLayer)
    {
        //遍历更改所有子物体layer
        trans.gameObject.layer = targetLayer;
        foreach (Transform child in trans)
        {
            ChangeLayer(child, targetLayer);
            Debug.Log(child.name + "子对象Layer更改成功！");
        }
    }


    public static bool AutoAddLayer(string layer)
    {
        if (!HasThisLayer(layer))
        {
            SerializedObject tagMagager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Tagmanager.asset"));
            SerializedProperty it = tagMagager.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.name.Equals("layers"))
                {
                    for (int i = 0; i < it.arraySize; i++)
                    {
                        if (i <= 7)
                        {
                            continue;
                        }
                        SerializedProperty sp = it.GetArrayElementAtIndex(i);
                        if (string.IsNullOrEmpty(sp.stringValue))
                        {
                            sp.stringValue = layer;
                            tagMagager.ApplyModifiedProperties();
                            Debug.Log("success");
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    static bool HasThisLayer(string layer)
    {
        //先清除已保存的
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Tagmanager.asset"));
        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name.Equals("layers"))
            {
                for (int i = 0; i < it.arraySize; i++)
                {
                    if (i <= 7)
                    {
                        continue;
                    }
                    SerializedProperty sp = it.GetArrayElementAtIndex(i);
                    if (!string.IsNullOrEmpty(sp.stringValue))
                    {
                        if (sp.stringValue.Equals(layer))
                        {
                            sp.stringValue = string.Empty;
                            tagManager.ApplyModifiedProperties();
                        }
                    }
                }
            }
        }
        for (int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++)
        {
            if (UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer))
            {
                return true;
            }
        }
        return false;
    }

}
