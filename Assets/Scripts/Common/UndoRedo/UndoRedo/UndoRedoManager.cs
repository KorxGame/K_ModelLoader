using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommandUndoRedo
{
    public static class UndoRedoManager
    {
        //static UndoRedo undoRedo = new UndoRedo();
        
        //不同面板有不同的撤销
        static Dictionary<string, UndoRedo> dic_undoRedo = new Dictionary<string, UndoRedo>();

        //public static int maxUndoStored {get {return undoRedo.maxUndoStored;} set {undoRedo.maxUndoStored = value;}}

        // public static void Clear()
        // {
        // 	undoRedo.Clear();
        // }
        //
        // public static void Undo()
        // {
        // 	undoRedo.Undo();
        // }
        //
        // public static void Redo()
        // {
        // 	undoRedo.Redo();
        // }
        //
        // public static void Insert(ICommand command)
        // {
        // 	undoRedo.Insert(command);
        // }
        //
        // public static void Execute(ICommand command)
        // {
        // 	undoRedo.Execute(command);
        // }
        public static void Clear(string key)
        {
            // undoRedo.Clear();
            if (dic_undoRedo.ContainsKey(key))
            {
                dic_undoRedo[key].Clear();
            }
        }

        public static void Undo(string key)
        {
            if (dic_undoRedo.ContainsKey(key))
            {
                dic_undoRedo[key].Undo();
            }
        }

        public static void Redo(string key)
        {
            if (dic_undoRedo.ContainsKey(key))
            {
                dic_undoRedo[key].Redo();
            }
        }

        public static void Insert(string key, ICommand command)
        {
            if (!dic_undoRedo.ContainsKey(key))
            {
                UndoRedo undoRedo = new UndoRedo();
                dic_undoRedo.Add(key, undoRedo);
            }

            dic_undoRedo[key].Insert(command);
        }

        public static void Execute(string key, ICommand command)
        {
            if (dic_undoRedo.ContainsKey(key))
            {
                dic_undoRedo[key].Execute(command);
            }
        }
    }
}