using System.Collections;
using System.Collections.Generic;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum tipType
{
    log,
    warning,
    error
}

public static class TipManager
{
    private static RectTransform rectText;
    private static RectTransform rectTextParent;
    private static ContentSizeFitter contentSizeFitter;
    
    private static TMP_Text tipText;

    public static TMP_Text TipText
    {
        get
        {
            if (tipText == null)
            {
                Transform canvas = GameObject.Find("Canvas").transform;
                if (canvas == null)
                {
                    Debug.Log("未找到:Canvas");
                    return null;
                }

                Transform ScrollView = canvas.FindChildByName("ScrollView");
                if (ScrollView == null)
                {
                    Debug.Log("未找到:ScrollView");
                    return null;
                }

                tipText = ScrollView.GetComponentInChildren<TMP_Text>();

                contentSizeFitter = tipText.GetComponent<ContentSizeFitter>();
                rectText = tipText.GetComponent<RectTransform>();
                rectTextParent = tipText.transform.parent.GetComponent<RectTransform>();
            }

            return tipText;
        }
    }


    public static void SetTip(this string text, tipType tipType = tipType.log)
    {
        TipText.text = TipText.text + "\n" + GetTextColor(text, tipType);

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectText);
        InitRect();
    }

    public static void SetLogTip(this string text)
    {
        //TipText.text = TipText.text + "\n" + GetTextColor(text, tipType.log);

        SetTip(text , tipType.log);

    }

    public static void SetWarningTip(this string text)
    {
        //TipText.text = TipText.text + "\n" + GetTextColor(text, tipType.warning);
        SetTip(text , tipType.warning);

    }

    public static void SetErrorTip(this string text)
    {
        //TipText.text = TipText.text + "\n" + GetTextColor(text, tipType.error);
        SetTip(text , tipType.error);

    }

    static string GetTextColor(string text, tipType tipType)
    {
        string str = "";
        switch (tipType)
        {
            case tipType.log:
                str = $"<color=white>{text}</color>";
                break;
            case tipType.warning:
                str = $"<color=yellow>{text}</color>";
                break;
            case tipType.error:
                str = $"<color=red>{text}</color>";
                break;
        }

        return str;
    }


    static void InitRect()
    {
        rectText.anchoredPosition = new Vector2(0, rectText.rect.height > rectTextParent.rect.height
            ? rectText.rect.height - rectTextParent.rect.height
            : 0f);
    }
}