using UnityEngine;

public class MSCamera : MonoBehaviour
{
    MSNetWorker netWorker;
    public ModGameMaster gameMaster { get; set; }
    public ModControl controller { get; set; }

    void Start()
    {
        GameObject player = GameObject.Find("Player");
        netWorker = new MSNetWorker(this);
        gameMaster = new ModGameMaster(this, player);
        controller = new ModControl(this);
        MSMain.modControl = controller;
        MSMain.modGameMaster = gameMaster;
        gameMaster.Start();
        netWorker.Start();
    }

    void Update()
    {   
        if (!MSMain.inited)
        {
            return;
        }

        netWorker.Update();
        gameMaster.Update();
        controller.Update();
    }

    void FixedUpdate()
    {
        if (!MSMain.inited)
        {
            return;
        }

        controller.FixedUpdate();
    }

    void OnDestory()
    {
        netWorker.Stop();
        gameMaster.Stop();
    }
}

