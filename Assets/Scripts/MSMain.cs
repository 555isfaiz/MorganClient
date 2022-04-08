using System;
using System.Collections.Generic;
using UnityEngine;

public class MSMain
{
    public static int currentSessionId;
    public static int mainPlayerId;
    public static Action<MSMessageBase> func_SendMsg;
    public static ModControl modControl;
    public static ModGameMaster modGameMaster;

    // for shooting demo
    public static bool inited = false;

    static void Init() 
    {
        modControl.Start();
        SModUIs modUIs = modGameMaster.GetSubMod(SModUIs.modName) as SModUIs;
        modUIs.WaitJoin();
        CSLogin msg = new CSLogin();
        func_SendMsg(msg);
    }

    public static void Login() 
    {
        Init();
    }

    public static void Quit() 
    {
        Application.Quit();
    }

    public static void GameJoined(int side, int gameSession, List<BPlayer> players)
    {
        inited = true;
        modGameMaster.FireGameJoin(side, gameSession, players);
        modControl.FireGameJoin();
    }
}