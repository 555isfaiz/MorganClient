using System.Collections.Generic;
using UnityEngine;

public class MsgHandler : SubModBase
{
    InputStream input = new InputStream();

    public MsgHandler(ModBase owner) : base(owner){}

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

    void handle(MSMessageBase msg)
    {
        int id = MSMessageBase.GetMessageId(msg);
        switch (id)
        {
            case 1003:
                OnSCJoinGame((SCJoinGame)msg);
                break;

            case 1004:
                OnSCLogin((SCLogin)msg);
                break;

            case 2002:
                OnSCMove(msg as SCMove);
                break;

            default:
                Debug.LogError("can't handle this message, msgId:" + id);
                break;
        }
    }

    void OnSCJoinGame(SCJoinGame msg)
    {
        MSCamera c = (MSCamera)(GetOwner().GetOwner());
        List<BPlayer> l = new List<BPlayer>();

        foreach (var m in msg.players)
        {
            l.Add((BPlayer)m);
        }
        c.gameMaster.NewGame(msg.mySide, msg.sessionId, l);
    }

    void OnSCLogin(SCLogin msg)
    {
        MSCamera c = (MSCamera)(GetOwner().GetOwner());
        c.gameMaster.SetMyId(msg.playerId);
    }

    void OnSCMove(SCMove msg)
    {
        Debug.Log("scmove get!");
    }
}