using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Common;
using TMPro;
using TriLibCore;
using UnityEngine;
using UnityEngine.UI;

public class ModelLoadTest : MonoBehaviour
{
    public BoundsCalculator.BoundsCalculationMethod boundsCalculator;
    public bool includeInactiveObjects;
    public bool isSecondPass;


    public float modelDefaultSize = 0.5f;
    //public TMP_Text Tip;

    public TMP_InputField InputField_modelPath;
    public Button Button_OpenFile;
    public Button Button_check;
    public Image loadimage;

    private AssetLoaderOptions assetLoaderOptions;

    private GameObject root;

    public GameObject Root
    {
        get
        {
            if (root == null)
            {
                GameObject go = new GameObject();
                go.name = "Root";
                root = go;
            }

            return root;
        }
    }

    #region 读取模型类型

    string fbxObjGltf = "模型文件(*.fbx; *.obj; *.glb;*.gltf;)\0*.fbx; *.obj; *.glb;*.gltf;\0";
    string allFile = "所有文件\0*;\0";

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //transform.FindChildByName("Tip").TryGetComponent(out Tip);
        //查找节点
        transform.FindChildByName("InputField_modelPath").TryGetComponent(out InputField_modelPath);
        transform.FindChildByName("Button_OpenFile").TryGetComponent(out Button_OpenFile);
        transform.FindChildByName("Button_check").TryGetComponent(out Button_check);
        Button_check.interactable = false;
        Button_check.transform.FindChildByName("load").TryGetComponent(out loadimage);
        //添加事件
        Button_OpenFile.onClick.AddListener(() =>
        {
            OpenFile(allFile, path =>
            {
                InputField_modelPath.text = path;

                Button_check.interactable = PathCheck(path);

                loadimage.fillAmount = 0;
            }, name => { });
        });

        InputField_modelPath.onEndEdit.AddListener((path) =>
        {
            Button_check.interactable = PathCheck(path);
            loadimage.fillAmount = 0;
        });


        assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        Button_check.onClick.AddListener(() =>
        {
            AssetLoader.LoadModelFromFile(InputField_modelPath.text, context => { print("onLoad"); },
                context =>
                {
                    if (context.RootGameObject != null)
                    {
                        string loadLog = $"模型加载成功:{context.RootGameObject.name}";
                        loadLog.SetLogTip();
                        
                        AutoAddCollider(context.RootGameObject.transform, context.Filename);
                        
                    }
                },
                (context, f) =>
                {
                    print("加载进度:" + f);
                    loadimage.fillAmount = f;
                }, error =>
                {
                    string loadErrorLog = $"模型加载失败:{error.GetInnerException().Message}";
                    loadErrorLog.SetErrorTip();
                    //Tip.text = error.GetInnerException().Message;
                },
                Root,
                assetLoaderOptions);
        });
    }

    // Update is called once per frame
    void Update()
    {
    }

    #region 打开文件选择窗口

    void OpenFile(string filter, Action<string> openfilePath, Action<string> openfileName)
    {
        OpenDialogFile openFileName = new OpenDialogFile();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.customFilter = fbxObjGltf;
        openFileName.filter = filter;
        //"模型文件(*.fbx; *.obj; *.glb;*.gltf;)\0*.fbx; *.obj; *.glb;*.gltf;\0";//指定筛选的文件格式，指定格式写在\0 \0中间

        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\'); //默认路径
        openFileName.title = "模型文件选择窗口";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
        openFileName.dlgOwner = GetActiveWindow(); //将窗口置于最前面
        if (DllOpenFileDialog.GetSaveFileName(openFileName))
        {
            Debug.Log("filePath" + openFileName.file); //文件路径
            Debug.Log("fileName" + openFileName.fileTitle); //文件名
            openfilePath?.Invoke(openFileName.file);
            openfileName?.Invoke(openFileName.fileTitle);
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    #endregion

    #region 加载模型

    void AutoAddCollider(Transform target, string path)
    {
        Bounds currentBounds = BoundsCalculator.CalculateBounds(target, out bool foundCanvas, boundsCalculator,
            includeInactiveObjects, !isSecondPass);

        print(path);
        string fileName = Path.GetFileName(path);
        print(fileName);

        bool readSize=  GetSizeByFileName(fileName, out float sizeByName);
        
        //限制最大值/最小值
        sizeByName= Mathf.Clamp(sizeByName, 0.1f, 3f);
      

        Vector3 size = currentBounds.size;
        float scale = VectorGetMaxValue(size);
        target.localScale = target.localScale / scale * sizeByName;


        BoxCollider box = target.gameObject.AddComponent<BoxCollider>();
        box.center = currentBounds.center;
        box.size = currentBounds.size;
        target.GetOrAddComponent<TriggerRectView>().currentBounds = currentBounds;

        
        #region 打印日志
        "正在获取文件名...".SetLogTip();
        fileName.SetLogTip();
        
        "正在根据文件名获取默认加载size...".SetLogTip();
        string sizeLog = readSize ? $"获取模型size成功:{sizeByName}" : $"获取模型size失败,设置默认size:{modelDefaultSize}";
        sizeLog.SetLogTip();
        
        "正在计算模型边界...".SetLogTip();
        "模型尺寸归一化...".SetLogTip();
        "模型添加碰撞器...".SetLogTip();
        
        
        #endregion
        //SizeNormalLized(box);
    }

    // void SizeNormalLized(BoxCollider collider)
    // {
    //     float scale = VectorGetMaxValue(collider.size);
    //     collider.transform.localScale /= scale;
    // }

    float VectorGetMaxValue(Vector3 target)
    {
        float v = target.x;
        if (target.y > v)
            v = target.y;
        if (target.z > v)
            v = target.z;
        return v;
    }


    bool GetSizeByFileName(string fileName, out float size)
    {
        string[] strs = fileName.SplitFromTo('(', ')');

        foreach (var str in strs)
        {
            if (str.Contains("size="))
            {
                if (float.TryParse(str.Replace("size=", String.Empty), out size))
                {
                    return true;
                }
            }
        }

        size = modelDefaultSize;
        return false;
    }

    #endregion


    #region 路经检测

    bool PathCheck(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            TipSet(TipType.PathEmpty);
            return false;
        }

        if (!File.Exists(path))
        {
            TipSet(TipType.NoFile);
            return false;
        }

        //string extName = GetExtName(path);
        string extName =Path.GetExtension(path);
        print(extName);
        if (!string.IsNullOrEmpty(extName)
            && !extName.Equals(".fbx")
            && !extName.Equals(".FBX")
            && !extName.Equals(".obj")
            && !extName.Equals(".OBJ")
            && !extName.Equals(".gltf")
            && !extName.Equals(".glb")
        )
        {
            TipSet(TipType.NoModelFile);
            return false;
        }

        TipSet(TipType.none);
        return true;
    }

    public string GetExtName(string fileName)
    {
        try
        {
            string extName;
            extName = fileName.Substring(fileName.LastIndexOf('.'));
            return extName;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    #endregion

    #region 消息提示

    public enum TipType
    {
        none,
        PathEmpty, //路径为空
        NoFile, //路径没有文件
        NoModelFile, //不是模型文件
    }

    void TipSet(TipType tipType)
    {
        string tipText = "";
        switch (tipType)
        {
            case TipType.none:
                tipText = "";
                break;
            case TipType.PathEmpty:
                tipText = "未输入路径!!!";
                break;
            case TipType.NoFile:
                tipText = "此路径无文件!!!";
                break;
            case TipType.NoModelFile:
                tipText = "此文件非特定模型文件!!!";
                break;
            default:
                tipText = "";
                break;
        }

        //Tip.text = tipText;
        tipText.SetWarningTip();
    }

    #endregion
}