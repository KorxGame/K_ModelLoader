using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Drag : MonoBehaviour
{
    private GameObject go;//射线碰撞的物体
    private Vector3 screenSpace;
    private Vector3 offset;
    private bool isdrage = false;
    void Start()
    {
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnBtnDown();
        }
        if (Input.GetMouseButton(0))
        {
            OnBtn();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnBtnUp();
        }
    }
    void OnBtnDown()
    {
//整体初始位置 Camera.main.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//从摄像机发出到点击坐标的射线
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            go = hitInfo.collider.gameObject;
            screenSpace = Camera.main.WorldToScreenPoint(go.transform.position);
            offset = go.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
        }
    }
    void OnBtn()
    {
        if(go==null)
            return;
        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
        go.transform.position = currentPosition;
        isdrage = true;
    }
    void OnBtnUp()
    {
//结束后，清空物体
        go = null;
        isdrage = false;
    }
}