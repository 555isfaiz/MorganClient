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

    private Vector3 dirVector;
 
	//mouse move
	private float fMouseX;
	private float fMouseY;
	public float speed = 5;
	public float bottomLimitAngle = 90;//the limit angle
	private float bottomLimit;//the cos value

    void Start()
    {
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
        player = GameObject.Find("Player");
        netWorker = new MSNetWorker(this);
        gameMaster = new ModGameMaster(this, player);
        controller = new ModControl(this);
        MSShare.modControl = controller;
        MSShare.modGameMaster = gameMaster;
        gameMaster.Start();
        dirVector = Vector3.Normalize(player.transform.position - transform.position);
		transform.position = player.transform.position + fixedDistance * (-dirVector);
        //mouse move initial
		fMouseX = 0;
		fMouseY = 0;
		bottomLimit = Mathf.Cos(bottomLimitAngle / 180 * Mathf.PI);
    }

    void Init() 
    {
        netWorker.Start();
        controller.Start();
        CSLogin msg = new CSLogin();
        msg.isShooter = MSShare.isShooter;
        netWorker.Send(msg);
        gameInited = true;
    }

    void Update()
    {   
        // if (!MSShare.inited)
        // {
        //     return;
        // }

        // if (MSShare.inited && !gameInited)
        // {
        //     Init();
        //     return;
        // }
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
        // if (!locked)
        // {
            // always keep same distance between camera and player
            // posv3.x = player.transform.position.x;
            // posv3.z = player.transform.position.z + fixedDistance;
        // }
        // else
        // {
        //     var pos3D = gameMaster.GetTargetPos();
        //     var targetPos = new Vector2(pos3D.x, pos3D.z);
        //     var current = new Vector2(transform.position.x, transform.position.z);
        //     var playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
        //     angle = Vector2.SignedAngle(playerPos - current, targetPos - playerPos);
        //     var originR = transform.rotation;
        //     float k = Vector2.Distance(playerPos, targetPos) / Mathf.Abs(fixedDistance);
        //     float deltaX = (targetPos.x - playerPos.x) / k;
        //     float deltaZ = (targetPos.y - playerPos.y) / k;

        //     posv3.x = playerPos.x - deltaX;
        //     posv3.z = playerPos.y - deltaZ;
        //     transform.rotation = Quaternion.Lerp(originR, Quaternion.LookRotation(pos3D - transform.position, Vector3.up), Time.deltaTime * 20);
        //     // transform.LookAt(pos3D);
        // }
        // transform.position = Vector3.Lerp(transform.position, posv3, Time.deltaTime * 1000);
        // transform.position = posv3;

        // look at player
        transform.LookAt(player.transform);

        //Camera Move
		fMouseX = Input.GetAxis("Mouse X");
		fMouseY = Input.GetAxis("Mouse Y");
        Debug.Log("mouse x:" + fMouseX + ", mouse y:" + fMouseY);
 
		//avoid dithering
		if (Vector3.Dot (-dirVector.normalized, -player.transform.up.normalized) > bottomLimit) {
			if (fMouseY > 0) {
				fMouseY = 0;
			};
		}

        //two types of parameters;
		//(axis,value)is rotate around the axis of the transform's position;
		// (position, axis, value)is rotate around the axis of the specific position;
		//Rotate Horizontal
		transform.RotateAround(player.transform.position + new Vector3(0f, 1.3f, 0f) ,player.transform.up, speed * fMouseX);
		//Rotate Vertical
		transform.RotateAround(player.transform.position + new Vector3(0f, 1.3f, 0f), -VerticalRotateAxis(dirVector), speed * fMouseY);
 
		//distance Control
		dirVector = Vector3.Normalize(player.transform.position - transform.position);
    }

    Vector3 VerticalRotateAxis(Vector3 dirVector){
		Vector3 player2Camera = -dirVector.normalized;
		float x = player2Camera.x;
		float z = player2Camera.z;
		Vector3 rotateAxis = Vector3.zero;
		rotateAxis.z = Mathf.Sqrt (x * x / (x * x + z * z));
		rotateAxis.x = Mathf.Sqrt (z * z / (x * x + z * z));
		if (x >= 0) {
			if (z >= 0) {
				rotateAxis.x = -rotateAxis.x;
			}
		} else {
			if (z >= 0) {
				rotateAxis.x = -rotateAxis.x;
				rotateAxis.z = -rotateAxis.z;
			} else {
				rotateAxis.z = -rotateAxis.z;
			}
		}
		// Debug.Log (rotateAxis);
		return rotateAxis;
	}

    public void QuickSwitch(Vector3 target)
    {
        var originR = transform.rotation;
        transform.LookAt(gameMaster.GetTargetPos());
        var toR = transform.rotation;
        transform.rotation = Quaternion.RotateTowards(originR, toR, Time.deltaTime * 150);
    }
}

