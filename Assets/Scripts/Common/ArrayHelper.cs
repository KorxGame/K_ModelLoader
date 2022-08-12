using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 数组助手类，对数组的改造和操作
    /// 提供数组常用功能
    /// </summary>
    public static  class ArrayHelper
    {
        //7个方法 查找 所有满足条件的所有对象
        //排序【升序，降序】 最大值，最小值，筛选
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">委托方法</param>
        /// <returns></returns>
        public static T Find<T>(this T[] array,Func<T,bool> condition)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                {
                    return array[i];
                }
            }
            return default(T);
        }

        //查找所有满足条件的数组元素
        //找敌人  血量大于79的敌人

        public static T[] FindAll<T>(this T[] array,Func<T,bool> condition)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                //if(array[i]>79)
                if (condition(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <typeparam name="T">代表数组数组类型</typeparam>
        /// <typeparam name="Q">比较条件的返回值类型</typeparam>
        /// <param name="array">比较的数组</param>
        /// <param name="condition">比较的方法</param>
        /// <returns></returns>
        public static T GetMax<T,Q>
            (this T[] array,Func<T,Q> condition) where Q:IComparable
        {
            if (array.Length == 0)
                return default;
            T maxIndex = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                //if(array[i]>maxIndex)
                if (condition(maxIndex).
                        CompareTo(
                            condition(array[i])) < 0)
                {
                    maxIndex = array[i];
                }
            }
            return maxIndex;
        }

        //最小值
        public static T GetMin<T, Q>
            (this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            if (array.Length == 0)
                return default;
            T minIndex = array[0];
            for (int i = 0; i < array.Length; i++)
            {
                //if(array[i]>maxIndex)
                if (condition(minIndex).
                        CompareTo(
                            condition(array[i])) > 0)
                {
                    minIndex = array[i];
                }
            }
            return minIndex;
        }
        //升序
        public static void Order_ascending<T, Q>(this T[] array,Func<T,Q> condition) where Q:IComparable
        {
            for (int i = 0; i < array.Length; i++) //每一次循环都会把最大值移到最后
            {
                for (int j = 0; j < array.Length-i-1; j++)
                {
                    if (condition(array[j]).CompareTo(condition(array[j+1]))>0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }

        }

        //降序
        public static void Order_descending<T, Q>(this T[] array, Func<T, Q> condition) where Q : IComparable
        {
            for (int i = 0; i < array.Length; i++) //每一次循环都会把最大值移到最后
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) < 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }

        }

        //筛选  从T[]中筛选Q[]
        public static Q[] Select<T, Q>(this T[] array,Func<T,Q> condition)
        {
            Q[] result = new Q[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = condition(array[i]);
            }
            return result;
        }

        

        

    }
}


