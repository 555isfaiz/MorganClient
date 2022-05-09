using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSMain
{
    public static int currentSessionId;
    public static int mainPlayerId;
    public static Action<MSMessageBase> func_SendMsg;
    public static ModControl modControl;
    public static ModGameMaster modGameMaster;
    public static MSNetWorker netWorker;

    public static bool single = true;
    // for shooting demo
    public static bool inited = false;

    static void Init() 
    {
        netWorker.Start();
        modControl.Start();
        SModUIs modUIs = modGameMaster.GetSubMod(SModUIs.modName) as SModUIs;
        modUIs.WaitJoin();
        CSLogin msg = new CSLogin();
        func_SendMsg(msg);
    }

    public static void Login() 
    {
        single = false;
        Init();
    }

    public static void StartSingle() 
    {
        single = true;
        netWorker.Start();
        modControl.Start();
        SModUIs modUIs = modGameMaster.GetSubMod(SModUIs.modName) as SModUIs;
        modUIs.WaitJoin();
        inited = true;
        List<BPlayer> players = new List<BPlayer>();
        BPlayer p = new BPlayer();
        p.playerId = 0;
        p.playerName = "player1";
        p.side = 0;
        p.curPos = new BVector3();
        p.curPos.x = 0;
        p.curPos.y = 0.7f;
        p.curPos.z = 0;
        players.Add(p);
        modGameMaster.FireGameJoin(0, 0, players);
        modControl.FireGameJoin();
    }

    public static void Quit() 
    {
        // somehow, the "netWorker.Stop()" in MSCamera.OnDestory() will not be triggered
        // doing this here saves my unity(ubuntu version) from freezing, although I dont know why...
        netWorker.Stop();
        Application.Quit();
    }

    public static void Reset()
    {
        // somehow, the "netWorker.Stop()" in MSCamera.OnDestory() will not be triggered
        // doing this here saves my Unity(ubuntu version) from freezing, although I dont know why...
        netWorker.Stop();
        MSMain.inited = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }

    public static void GameJoined(int side, int gameSession, List<BPlayer> players)
    {
        inited = true;
        modGameMaster.FireGameJoin(side, gameSession, players);
        modControl.FireGameJoin();
    }
}