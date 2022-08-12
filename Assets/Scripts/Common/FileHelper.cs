using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using UnityEngine;

public class FileHelper
{
    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="Path">文件路径</param>
    /// <param name="Strings">文件内容</param>
    /// <param name="append">添加模式</param>
    public static void WriteFile(string Path, string Strings, bool append = false)
    {
        if (!File.Exists(Path))
        {
//System.IO.File.Create(Path).Close();
            File.Create(Path).Close();
        }

        if (append)
        {
            File.AppendAllText(Path, Strings);
        }
        else
        {
            File.WriteAllText(Path, Strings);
        }
    }

    /// <summary>
    /// 读文件
    /// </summary>
    /// <param name="Path">文件路径</param>
    /// <returns></returns>
    public static bool ReadFile(string Path , out string content)
    {
        string s = "";
        if (!System.IO.File.Exists(Path))
        {
            Debug.Log("本地没有想要的文件:" + Path);
            content = null;
            return false;
        }
        else
        {
            StreamReader f2 = new StreamReader(Path, System.Text.Encoding.UTF8);
            try
            {
                s = f2.ReadToEnd();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            finally
            {
                f2.Close();
                f2.Dispose();
            }
        }

        content = s;
        return true;
    }

    public static List<string> ReadFileLine(string Path)
    {
        List<string> s = new List<string>();
        if (!System.IO.File.Exists(Path))
            Debug.LogError("不存在相应的目录");
        else
        {
            StreamReader f2 = new StreamReader(Path, System.Text.Encoding.UTF8);
            try
            {
                string line = f2.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
//Debug.LogError(line);
                    s.Add(line);
                    line = f2.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
            finally
            {
                f2.Close();
                f2.Dispose();
            }
        }

        return s;
    }

    public static Dictionary<string, Dictionary<string, string>> ReadExcel(string Path)
    {
        Dictionary<string, Dictionary<string, string>> configs = new Dictionary<string, Dictionary<string, string>>();
        List<string> s = ReadFileLine(Path);
        string[] title = s[0].Split(',');
        for (int i = 2; i < s.Count; i++)
        {
            string[] lineStr = s[i].Split(',');
            if (lineStr.Length == title.Length)
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                for (int j = 0; j < lineStr.Length; j++)
                {
                    dic.Add(title[j], lineStr[j]);
                }

                configs.Add(dic[title[0]], dic);
            }
            else
            {
                Debug.LogErrorFormat("{0}表头和实际数据不一致 index={1}", Path, i);
            }
        }

        return configs;
    }

    /// <summary>
    /// 存储AB文件到本地
    /// </summary>
    public static void SaveAsset(string filePath, byte[] bytes, int count)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        FileStream fs = fileInfo.Create();
        fs.Write(bytes, 0, count);
        fs.Flush();
        fs.Close();
        fs.Dispose();
        Debug.Log(filePath + "下载并存储完成");
    }
}