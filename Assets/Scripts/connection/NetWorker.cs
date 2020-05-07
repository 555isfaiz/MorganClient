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
    public ConcurrentQueue<byte[]> recvQ;
    public MSNetWorker(MonoBehaviour owner) : base(owner)
    {
        s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        recvQ = new ConcurrentQueue<byte[]>();
    }

    public override void StartOverride()
    {
        ts = new ThreadStart(UpdateOverride);
        worker = new Thread(ts);
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        s.Connect(new IPEndPoint(ip, 13139));
        start = true;
        MsgHandler msgHandler = new MsgHandler(this);
        AddSubMod("MsgHandler", msgHandler);
        worker.Start();
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
        OutputStream outs = new OutputStream();
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
        worker.Abort();
        s.Close();
    }

    public override void UpdateOverride()
    {
        while (start)
        {
            byte[] buffer = new byte[1024];
            int len = s.Receive(buffer) - 4;
            byte[] msg = new byte[len];
            Array.Copy(buffer, 4, msg, 0, len);
            recvQ.Enqueue(msg);
        }
    }
}