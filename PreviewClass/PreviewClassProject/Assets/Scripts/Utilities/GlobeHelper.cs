using UnityEngine;
using System.Collections.Generic;

using System;

public class GlobeHelper : MonoBehaviour
    {

    #region 常量
    public static class Constants
    {
        //战士场景名称
        public static string WarriorScene = "WarriorScene";

        //战士技能路径
        public static string WarriorResRoute = "Prefabs/MapContents/1001";

        //萝莉场景名称
        public static string LoliScene = "LoliScene";

        //萝莉技能路径
        public static string LoliResRoute = "Prefabs/MapContents/1002";

        //主角路径
        public static string MajorRoute = "Prefabs/Characters/Man/AM_CH_ZS_Base";

        //小骷髅路径
        public static string SkullSoliderRoute = "Prefabs/Enemy/Skull_Solider/AM_AC_SkullSolider";

        //小骷髅AI数据路径
        public static string SkullSoliderAIDataRoute = "Prefabs/Enemy/Skull_Solider/SkullSoliderAIData";


        //战士角色ID
        public static int Warrior_RoleID = 101;

        //小怪1号 - SkullSolider
        public static int SkullSolider_RoleID = 201;


    }
    #endregion


    #region 通用接口

    public static DateTime GetDateTimeNow()
    {
        try
        {
            return DateTime.Now;
        }
        catch (Exception)
        {
            return DateTime.Now;
        }
    }

    public static float RandomFloat(float min, float max)
    {
        return UnityEngine.Random.Range((int)(min * 100f), (int)(max * 100f) + 1) / 100f;
    }

    public static int RandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }

    public static List<T> GetComponentsAll<T>(GameObject go, bool includeInactive) where T : Component
        {
            List<T> listT = new List<T>();
            if (go != null)
            {
                T t = go.GetComponent<T>();
                if (t != null)
                {
                    listT.Add(t);
                }
                T[] ts = go.GetComponentsInChildren<T>(includeInactive);
                if (ts != null)
                {
                    for (int i = 0, max = ts.Length; i < max; i++)
                    {
                        T tt = ts[i];
                        listT.Add(tt);
                    }
                }
            }
            return listT;
        }

    #endregion





}




