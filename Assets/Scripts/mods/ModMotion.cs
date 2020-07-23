using System.Collections.Generic;
using System;
using UnityEngine;

public class ModMotion : ModBase
{
    GameObject go;
    Rigidbody rb;
    float moveSpeed = 5.0f;
    float groundY = 0.7f;
    float g = 25.0f;
    float premitiveV = 10.0f;
    float dashSpeed = 20.0f;
    long dashStart;
    long dashEnd;
    long singleDashDuration = 300;
    long maxDashDuration = 600;
    Vector3 dashDirection = new Vector3(0, 0, 0);
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
        DoDash();
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
        // move directions
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.MOVE_FORWARD))
        {
            // not dashing, then move
            if (dashEnd == 0)
            {
                direction += forward;
            }
        }
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.MOVE_BACKWARD))
        {
            if (dashEnd == 0)
            {
                direction += back;
            }
        }
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.MOVE_LEFT))
        {
            if (dashEnd == 0)
            {
                direction += left;
            }
        }
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.MOVE_RIGHT))
        {
            if (dashEnd == 0)
            {
                direction += right;
            }
        }
        // dash directions
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.DASH_FORWARD))
        {
            direction = new Vector3(0, 0, 0);
            StartDash(forward);
        }
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.DASH_BACKWARD))
        {
            direction = new Vector3(0, 0, 0);
            StartDash(back);
        }
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.DASH_LEFT))
        {
            direction = new Vector3(0, 0, 0);
            StartDash(left);
        }
        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.DASH_RIGHT))
        {
            direction = new Vector3(0, 0, 0);
            StartDash(right);
        }

        if (direction.x != 0.0f || direction.y != 0.0f || direction.z != 0.0f)
        {
            if (lastErrorCode != -1)
            {
                go.transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
            MSShare.func_SendMsg(GetCSMove(direction));
        }

        if (MSShare.modControl.TryExecuteCommand(ModControl.Command.JUMP))
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

    void StartDash(Vector3 direction)
    {
        dashDirection += direction;
        if (dashStart == 0)
        {
            dashStart = Utils.GetTimeMilli();
            dashEnd = dashStart + singleDashDuration;
        }
        else if ((dashEnd - dashStart) <= maxDashDuration)
        {
            dashEnd = Math.Min(dashEnd + singleDashDuration, dashStart + maxDashDuration);
        }
    }

    void DoDash()
    {
        if (dashEnd == 0)
        {
            return;
        }

        go.transform.Translate(dashDirection * dashSpeed * Time.deltaTime);
        long now = Utils.GetTimeMilli();
        Debug.Log("dashing dir.x:" + dashDirection.x + ", dir.y:" + dashDirection.y + ", dir.z:" + dashDirection.z + "!!!!!!!!!!!!!!!!!!");
        if (now >= dashEnd)
        {
            dashEnd = 0L;
            dashStart = 0L;
            dashDirection.x = 0;
            dashDirection.y = 0;
            dashDirection.z = 0;
            Debug.Log("dash end~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
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
        // Debug.Log("pos from server x:" + pos.x + " y:" + pos.y + " z:" + pos.z + " pos now x:" + go.transform.position.x + " y:" + go.transform.position.y + " z:" + go.transform.position.z);
    }

    public void OnSCJump()
    {
        StartJump();
    }
}