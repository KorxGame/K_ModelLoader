using System;
using CommandUndoRedo;
using UnityEngine;

public class TransformCommand : ICommand
{
   public TransformValues newValues;
   public TransformValues oldValues;

    public Transform transform;
    //TransformGizmo transformGizmo;


    public TransformCommand(Transform transform) //TransformGizmo transformGizmo,
    {
        this.transform = transform;

        oldValues = new TransformValues()
            {localPosition = transform.localPosition, localRotation = transform.localRotation, localScale = transform.localScale};
    }

    public void StoreNewTransformValues()
    {
        newValues = new TransformValues()
            {localPosition = transform.localPosition, localRotation = transform.localRotation, localScale = transform.localScale};

        // if (CheckOldAndNew())
        // {
        //     UndoRedoManager.Insert(this);
        // }
    }

    public void Execute()
    {
        if (transform != null)
        {
            transform.localPosition = newValues.localPosition;
            transform.localRotation = newValues.localRotation;
            transform.localScale = newValues.localScale;
        }
    }

    public void UnExecute()
    {
        if (transform != null)
        {
            transform.localPosition = oldValues.localPosition;
            transform.localRotation = oldValues.localRotation;
            transform.localScale = oldValues.localScale;
        }
    }

    public struct TransformValues
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
    }

    public bool CheckOldAndNew()
    {
        if (newValues.localPosition == oldValues.localPosition && newValues.localRotation == oldValues.localRotation &&
            newValues.localScale == oldValues.localScale)
            return false;
        else
            return true;
    }
   
}