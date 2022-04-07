using System;

public class MSMain
{
    public static int currentSessionId;
    public static int mainPlayerId;
    public static Action<MSMessageBase> func_SendMsg;
    public static ModControl modControl;
    public static ModGameMaster modGameMaster;

    // for shooting demo
    public static bool inited = false;
    public static bool isShooter;

    static void Init() 
    {
        modControl.Start();
        SModUIs modUIs = modGameMaster.GetSubMod(SModUIs.modName) as SModUIs;
        modUIs.PreInit();
        CSLogin msg = new CSLogin();
        func_SendMsg(msg);
        inited = true;
    }

    public static void OnClickShooter() 
    {
        isShooter = true;
        Init();
    }

    public static void OnClickTarget() 
    {
        isShooter = false;
        Init();
    }
}