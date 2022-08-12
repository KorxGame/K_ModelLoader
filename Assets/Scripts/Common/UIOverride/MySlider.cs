using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MySlider : Slider ,IBeginDragHandler ,IEndDragHandler
{
   
    public Action beiginDrag { get; set; }
    public Action endDrag { get; set; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beiginDrag?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endDrag?.Invoke();
    }
    
    
}
