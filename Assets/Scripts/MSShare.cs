using System;
using UnityEngine;

public class MSShare
{
    public static int currentSessionId;
    public static int mainPlayerId;
    public static Action<MSMessageBase> func_SendMsg;
    public static ModControl modControl;
    public static ModGameMaster modGameMaster;

    // for shooting demo
    public static bool inited = false;
    public static bool isShooter;

    static void PreInit() 
    {
        ModUIs modUIs = modGameMaster.GetSubMod(ModUIs.modName) as ModUIs;
        modUIs.PreInit();
    }

    public static void OnClickShooter() 
    {
        isShooter = true;
        PreInit();
    }

    public static void OnClickTarget() 
    {
        isShooter = false;
        PreInit();
    }
}