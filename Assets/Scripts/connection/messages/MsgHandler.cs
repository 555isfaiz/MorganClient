using System.Collections.Generic;
using UnityEngine;

public class MsgHandler : SubModBase
{
    InputStream input = new InputStream();

    public static string modName = "MsgHandler";

    public MsgHandler(ModBase owner) : base(owner, "MsgHandler"){}

    public override void Start(){}

    public override void Update()
    {
        MSNetWorker nw = (MSNetWorker)GetOwner();
        byte[] buf;
        while (nw.recvQ.TryDequeue(out buf))
        {
            input.setBuffer(buf);
            MSMessageBase msg = input.read<MSMessageBase>();
            if (msg == null)
            {
                return;
            }
            handle(msg);
            input.reset();
            input.resetCursor();
            buf = null;
        }
    }

    public override void Stop(){}

    ModGameMaster GameMaster()
    {
        return (GetOwner().GetOwner() as MSCamera).gameMaster;
    }

    void handle(MSMessageBase msg)
    {
        int id = MSMessageBase.GetMessageId(msg);
        // Debug.Log("in message, id=" + id);
        switch (id)
        {
            case 1003:
                OnSCJoinGame((SCJoinGame)msg);
                break;

            case 1004:
                OnSCLogin((SCLogin)msg);
                break;

            case 1005:
                OnSCGameSync(msg as SCGameSync);
                break;

            case 2002:
                OnSCMove(msg as SCMove);
                break;

            case 2004:
                OnSCJump(msg as SCJump);
                break;

            case 2006:
                OnSCDashStart(msg as SCDashStart);
                break;

            case 2007:
                OnSCDashStop(msg as SCDashStop);
                break;

            default:
                Debug.LogError("can't handle this message, msgId:" + id);
                break;
        }
    }

    void OnSCJoinGame(SCJoinGame msg)
    {
        List<BPlayer> l = new List<BPlayer>();

        foreach (var m in msg.players)
        {
            l.Add((BPlayer)m);
        }
        MSMain.GameJoined(msg.mySide, msg.sessionId, l);
    }

    void OnSCLogin(SCLogin msg)
    {
        GameMaster().SetMyId(msg.playerId);
        MSGlobalParams.serverTimeZoneOffset = msg.serverTimeZone;
    }

    void OnSCMove(SCMove msg)
    {
        var gm = GameMaster();
        var other = gm.GetPlayerObject(msg.playerId);
        Vector3 pos = new Vector3(msg.curPos.x, msg.curPos.y, msg.curPos.z);
        Vector3 dir = new Vector3(msg.direction.x, msg.direction.y, msg.direction.z);
        if (msg.playerId == MSMain.mainPlayerId)
        {
            (other as MSHero).modMotion.OnSCMove(pos, dir, msg.result);
        }
        else
        {
            (other as MSOtherPlayer).modMotion.OnSCMove(pos, dir, msg.result);
        }
    }

    void OnSCJump(SCJump msg)
    {
        var gm = GameMaster();
        var other = gm.GetPlayerObject(msg.playerId);
        (other as MSOtherPlayer).modMotion.OnSCJump();
    }

    void OnSCGameSync(SCGameSync msg)
    {
        List<BPlayer> l = new List<BPlayer>();

        foreach (var m in msg.players)
        {
            l.Add((BPlayer)m);
        }
        GameMaster().SessionSync(msg.sessionId, l);
    }

    void OnSCDashStart(SCDashStart msg)
    {
        if (msg.playerId == MSMain.mainPlayerId)
        {
            return;
        }
        var gm = GameMaster();
        var player = gm.GetPlayerObject(msg.playerId) as MSOtherPlayer;
        Vector3 dir = new Vector3(msg.direction.x, msg.direction.y, msg.direction.z);
        player.modMotion.SCDashStart(dir);
    }

    void OnSCDashStop(SCDashStop msg)
    {
        if (msg.playerId == MSMain.mainPlayerId)
        {
            return;
        }
        var gm = GameMaster();
        var player = gm.GetPlayerObject(msg.playerId) as MSOtherPlayer;
        Vector3 pos = new Vector3(msg.finalPos.x, msg.finalPos.y, msg.finalPos.z);
        player.modMotion.SCDashEnd(pos);
    }
}