using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialHelper
{
    
    //暂时 Transform 只能存储对应一个名字 对应的material  后期修改
    private static Dictionary<Transform, List<Material>> dic_materials = new Dictionary<Transform, List<Material>>();

    public static List<Material> GetAllMaterialByName(this Transform transform, string matName)
    {
        List<Material> Material = new List<Material>();
        if (!dic_materials.ContainsKey(transform) || dic_materials[transform] == null )
        {
            Material = GetAllMaterialByName_base(transform , matName);
            dic_materials.Add(transform , Material);

            return Material;
        }
        else
        {
            return dic_materials[transform];
        }
        
        
    }
    
    
     static List<Material> GetAllMaterialByName_base(this Transform transform, string matName)
    {
        List<Material> Material = new List<Material>();
        MeshRenderer[] mr = transform.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < mr.Length; i++)
        {
            for (int j = 0; j < mr[i].materials.Length; j++)
            {
                //Debug.Log(mr[i].materials[j].name);
                //删除" (Instance)"    
                string oldname = mr[i].materials[j].name;
                string name = oldname.Remove(oldname.Length - 11, 11);
                if (name == matName)
                {
                    Material.Add(mr[i].materials[j]);
                }
            }
        }

        return Material;
    }
}