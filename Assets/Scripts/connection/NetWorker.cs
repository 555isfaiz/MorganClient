using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using UnityEngine;

public class MSNetWorker : ModBase
{
    Thread worker;
    Socket s;
    bool start = false;
    ThreadStart ts;
    OutputStream outs = new OutputStream();
    public ConcurrentQueue<byte[]> recvQ;
    public MSNetWorker(MonoBehaviour owner) : base(owner, "MSNetWorker")
    {
        s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        recvQ = new ConcurrentQueue<byte[]>();
    }

    public override void StartOverride()
    {
        if (MSMain.single)
        {
            MSMain.func_SendMsg = (MSMessageBase msg) => {};
            return;
        }

        IPAddress ip = IPAddress.Parse("127.0.0.1");
        s.Connect(new IPEndPoint(ip, 13139));
        ts = new ThreadStart(UpdateOverride);
        worker = new Thread(ts);
        start = true;
        MsgHandler msgHandler = new MsgHandler(this);
        AddSubMod(MsgHandler.modName, msgHandler);
        worker.Start();
        MSMain.func_SendMsg = Send;
    }

    public override void Update()
    {
        foreach (var pair in subMods)
        {
            pair.Value.Update();
        }
    }

    public void Send(MSMessageBase msg)
    {
        outs.reset();
        outs.write<MSMessageBase>(msg);
        Send(outs.getBuffer());
    }

    public void Send(byte[] msg)
    {
        byte[] buffer = new byte[msg.Length + 4];
        Array.Copy(Utils.IntToBytes(msg.Length), 0, buffer, 0, 4);
        Array.Copy(msg, 0, buffer, 4, msg.Length);
        s.Send(buffer);
    }

    public override void StopOverride()
    {
        start = false;
        if (worker != null)
            worker.Abort();
        if (s != null)
            s.Close();
    }

    public override void UpdateOverride()
    {
        while (start)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int len = s.Receive(buffer);
                while (len > 0)
                {
                    int packLen = Utils.BytesToInt(buffer) - 4;
                    byte[] msg = new byte[packLen];
                    Array.Copy(buffer, 4, msg, 0, packLen);
                    recvQ.Enqueue(msg);
                    len -= (packLen + 4);
                    if (len > 0)
                    {
                        Array.Copy(buffer, packLen + 4, buffer, 0, len);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                start = false;
                break;
            }
        }
    }
}