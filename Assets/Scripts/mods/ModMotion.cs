using System.Collections.Generic;
using UnityEngine;

public class ModMotion : ModBase
{
    GameObject go;
    Rigidbody rb;
    public float moveSpeed = 5.0f;
    public float groundY = 0.7f;
    public float g = 25.0f;
    public float premitiveV = 10.0f;
    public float jumpStartTime;
    public int jumpPhase;
    public float nextJumpable;
    Vector3 forward;
    Vector3 back;
    Vector3 left;
    Vector3 right;
    bool controlable;
    int lastErrorCode = 0;

    public ModMotion(MonoBehaviour owner, GameObject go) : base(owner)
    {
        this.go = go;
        if (owner is MSHero)
        {
            controlable = true;
        }
        else
        {
            controlable = false;
        }
    }

    public override void StartOverride()
    {
        rb = go.GetComponent<Rigidbody>();
        jumpStartTime = 0.0f;
        jumpPhase = 0;
    }

    public override void UpdateOverride()
    {
        if (controlable)
        {
            UpdateDirection();
            UpdatePosition();
        }
        DoJump();
    }

    public override void StopOverride() {}

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Ground"))
        {
            jumpPhase = 0;
            jumpStartTime = 0f;
            nextJumpable = Time.time + 0.05f;
        }
    }

    public void OnCollisionExit(Collision collision)
    {

    }

    public void OnCollisionStay(Collision collision)
    {

    }

    void UpdateDirection()
    {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        var delta = go.transform.position - camera.transform.position;
        forward = Utils.Unitlize(new Vector3(delta.x, 0, delta.z));
        back = Vector3.Reflect(forward, forward);
        right = Utils.Unitlize(new Vector3(delta.z, 0f, -delta.x));
        left = Vector3.Reflect(right, right);
    }

    void UpdatePosition()
    {
        var direction = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            direction += forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += right;
        }

        if (direction.x != 0.0f || direction.y != 0.0f || direction.z != 0.0f)
        {
            if (lastErrorCode != -1)
            {
                go.transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
            MSShare.func_SendMsg(GetCSMove(direction));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CSJump csjump = new CSJump();
            csjump.playerId = MSShare.mainPlayerId;
            MSShare.func_SendMsg(csjump);
            StartJump();
        }
    }
    
    void StartJump()
    {
        if ((jumpPhase < 2) && (nextJumpable < Time.time))
        {
            jumpPhase = jumpPhase + 1;
            jumpStartTime = Time.time;
            var pv = new Vector3(0.0f, premitiveV, 0.0f);
            rb.velocity = pv;
        }
    }

    void DoJump()
    {
        if (jumpPhase == 0)
        {
            return;
        }
        float deltaTime = Time.time - jumpStartTime;
        var speed = GetJumpSpeed(deltaTime);
        rb.velocity = speed;
    }

    Vector3 GetJumpSpeed(float deltaTime)
    {
        var v3 =  new Vector3(0.0f, 0.0f, 0.0f);
        v3.y = premitiveV - g * deltaTime;
        return v3;
    }

    CSMove GetCSMove(Vector3 direction)
    {
        CSMove msg = new CSMove();
        msg.playerId = MSShare.mainPlayerId;
        msg.curPos = new BVector3();
        msg.curPos.x = go.transform.position.x;
        msg.curPos.y = go.transform.position.y;
        msg.curPos.z = go.transform.position.z;
        msg.direction = new BVector3();
        msg.direction.x = direction.x;
        msg.direction.y = direction.y;
        msg.direction.z = direction.z;
        msg.timeStamp = Utils.GetTimeMilli();
        return msg;
    }

    public void OnSCMove(Vector3 pos, Vector3 direction, int errorCode)
    {
        if (!controlable)
        {
            Vector3 y = new Vector3(pos.x, jumpPhase == 0 ? pos.y : go.transform.position.y, pos.z);
            go.transform.position = y;
            Quaternion newRotation = Quaternion.LookRotation(direction);
            go.transform.rotation = newRotation;
        }
        else
        {
            if (Vector3.Distance(pos, go.transform.position) > 1/* || errorCode != 0*/)
                go.transform.position = pos;
            lastErrorCode = errorCode;
            // if (lastErrorCode != 0)
            // {
            //     Debug.Log("lastErrorCode:" + lastErrorCode);
            // }
        }
        Debug.Log("pos from server x:" + pos.x + " y:" + pos.y + " z:" + pos.z + " pos now x:" + go.transform.position.x + " y:" + go.transform.position.y + " z:" + go.transform.position.z);
    }

    public void OnSCJump()
    {
        StartJump();
    }
}