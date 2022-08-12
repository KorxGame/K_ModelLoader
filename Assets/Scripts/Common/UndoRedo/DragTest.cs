using System.Collections;
using System.Collections.Generic;
using CommandUndoRedo;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTest : MonoBehaviour
{
    private GameObject dragGo;//射线碰撞的物体
    private Vector3 screenSpace;
    private Vector3 offset;

    private bool isdrage = false;

    //用于撤销恢复
    private Vector3 startPos;
    private TransformCommand transformCommand;
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

        if (Input.GetKeyDown(KeyCode.A))
        {
            UndoRedoManager.Redo("test");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UndoRedoManager.Undo("test");
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
            dragGo = hitInfo.collider.gameObject;
            screenSpace = Camera.main.WorldToScreenPoint(dragGo.transform.position);
            offset = dragGo.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));

            transformCommand = new TransformCommand(dragGo.transform);
        }
    }
    
    void OnBtn()
    {
        if(dragGo==null)
            return;
        Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;

        dragGo.transform.position = currentPosition;
        isdrage = true;
    }
    void OnBtnUp()
    {
        if (dragGo != null )
        {
            transformCommand.StoreNewTransformValues();
            if (transformCommand.CheckOldAndNew())
            {
                UndoRedoManager.Insert( "test" , transformCommand);
            }
        }
      
        
        //结束后，清空物体
        dragGo = null;
        isdrage = false;
    }
    
    
}