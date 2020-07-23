using System;
public class MSShare
{
    public static int currentSessionId;
    public static int mainPlayerId;
    public static Action<MSMessageBase> func_SendMsg;
    public static ModControl modControl;
    public static ModGameMaster modGameMaster;
}