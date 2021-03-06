using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSCamera : MonoBehaviour
{
    public bool locked = false;
    public float fixedHeight = 1.3f;
    public float fixedDistance = -5.0f;
    public float moveAngle = 30f;
    GameObject player;
    MSNetWorker netWorker;
    public ModGameMaster gameMaster { get; set; }
    public ModControl controller { get; set; }
    public float angle;
    public bool gameInited = false;

    void Start()
    {
        player = GameObject.Find("Player");
        netWorker = new MSNetWorker(this);
        gameMaster = new ModGameMaster(this, player);
        controller = new ModControl(this);
        MSShare.modControl = controller;
        MSShare.modGameMaster = gameMaster;
        gameMaster.Start();
    }

    void Init() 
    {
        netWorker.Start();
        controller.Start();
        CSLogin msg = new CSLogin();
        netWorker.Send(msg);
        gameInited = true;
    }

    void Update()
    {   
        if (!MSShare.inited)
        {
            return;
        }

        if (MSShare.inited && !gameInited)
        {
            Init();
            return;
        }
        netWorker.Update();
        gameMaster.Update();
        controller.Update();
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    void OnDestory()
    {
        netWorker.Stop();
        gameMaster.Stop();
    }

    void FollowPlayer()
    {
        var posv3 = new Vector3(transform.position.x, fixedHeight + player.transform.position.y, transform.position.z);
        if (!locked)
        {
            posv3.x = player.transform.position.x;
            posv3.z = player.transform.position.z + fixedDistance;
        }
        else
        {
            var pos3D = gameMaster.GetTargetPos();
            var targetPos = new Vector2(pos3D.x, pos3D.z);
            var current = new Vector2(transform.position.x, transform.position.z);
            var playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
            angle = Vector2.SignedAngle(playerPos - current, targetPos - playerPos);
            var originR = transform.rotation;
            float k = Vector2.Distance(playerPos, targetPos) / Mathf.Abs(fixedDistance);
            float deltaX = (targetPos.x - playerPos.x) / k;
            float deltaZ = (targetPos.y - playerPos.y) / k;

            posv3.x = playerPos.x - deltaX;
            posv3.z = playerPos.y - deltaZ;
            transform.rotation = Quaternion.Lerp(originR, Quaternion.LookRotation(pos3D - transform.position, Vector3.up), Time.deltaTime * 20);
            // transform.LookAt(pos3D);
        }
        transform.position = Vector3.Lerp(transform.position, posv3, Time.deltaTime * 1000);
        // transform.position = posv3;
    }

    public void QuickSwitch(Vector3 target)
    {
        var originR = transform.rotation;
        transform.LookAt(gameMaster.GetTargetPos());
        var toR = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(originR, toR, Time.deltaTime * 150);
    }
}

