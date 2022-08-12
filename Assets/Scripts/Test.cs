using System.Collections;
using System.Collections.Generic;
using TMPro;
using TriLibCore;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public BoundsCalculator.BoundsCalculationMethod boundsCalculator;
    public bool includeInactiveObjects;
    public bool isSecondPass;


    private string modelPath;
    public TMP_Dropdown dropDown;
    public Button loadToScene;
    public Button addCollider;
    public Button sizeNormalize;

    public Button loadAuto;

    public AssetLoaderOptions assetLoaderOptions;


    private GameObject root;
    private Dictionary<int, GameObject> modelTest = new Dictionary<int, GameObject>();

    public Image loadImage;

    public TMP_InputField input;

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


    // Start is called before the first frame update
    void Start()
    {
        //assetLoaderOptions = Resources.Load<AssetLoaderOptions>("");

        modelPath = Application.streamingAssetsPath;
        print(modelPath);
        loadToScene.onClick.AddListener(() =>
        {
            string modelName;
            if (dropDown.value == 0)
            {
                modelName = input.text + ".fbx";
            }
            else if (dropDown.value == 1)
            {
                modelName = input.text + ".obj";
            }
            else
            {
                modelName = input.text + ".glb";
            }

            string path = $"{modelPath}/{modelName}";

            LoadModelFromFile(path, Root, dropDown.value);
        });
        addCollider.onClick.AddListener(() =>
        {
            Debug.Log("addCollider");

            if (modelTest.ContainsKey(dropDown.value) && modelTest[dropDown.value] != null)
            {
                Debug.Log(modelTest[dropDown.value].transform.name);
                AutoAddCollider(modelTest[dropDown.value].transform);
            }
            else
            {
                Debug.Log("未加载");
            }
        });
        sizeNormalize.onClick.AddListener(() =>
        {
            if (modelTest.ContainsKey(dropDown.value) && modelTest[dropDown.value] != null)
            {
                float scale = VectorGetMaxValue(modelTest[dropDown.value].GetComponent<BoxCollider>().size);
                modelTest[dropDown.value].transform.localScale /= scale;
            }
            else
            {
                Debug.Log("未加载");
            }
        });


        loadAuto.onClick.AddListener(() =>
        {
            
        });
    }


    void LoadModelFromFile(string path, GameObject parent, int value)
    {
        AssetLoader.LoadModelFromFile(path, context => { print("onLoad"); }, context =>
            {
                if (context.RootGameObject != null)
                {
                    Debug.Log("Model successfully loaded." + context.RootGameObject.name);
                    if (!modelTest.ContainsKey(value))
                    {
                        modelTest.Add(value, context.RootGameObject);
                    }
                }
            },
            (context, f) =>
            {
                print("加载进度:" + f);
                loadImage.fillAmount = f;
            }, ONError,
            parent, assetLoaderOptions);
    }


    private void ONError(IContextualizedError obj)
    {
        print("ONError" + obj.GetInnerException().Message);
    }


    void AutoAddCollider(Transform target)
    {
        Bounds currentBounds = BoundsCalculator.CalculateBounds(target, out bool foundCanvas, boundsCalculator,
            includeInactiveObjects, !isSecondPass);
        //
        // Vector3 size = currentBounds.size;
        // float scale = VectorGetMaxValue(size);
        // target.localScale /= scale;
        BoxCollider box = target.gameObject.AddComponent<BoxCollider>();
        box.center = currentBounds.center;
        box.size = currentBounds.size;
    }

    float VectorGetMaxValue(Vector3 target)
    {
        float v = target.x;
        if (target.y > v)
            v = target.y;
        if (target.z > v)
            v = target.z;
        return v;
    }
}