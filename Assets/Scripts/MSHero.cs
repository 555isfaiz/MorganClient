using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MSHero : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float groundY = 0.7f;
    public float g = 25.0f;
    public float premitiveV = 10.0f;
    public float jumpStartTime;
    public int jumpPhase;
    public float nextJumpable;
    Rigidbody rb;
    GameObject go;
    Vector3 forward;
    Vector3 back;
    Vector3 left;
    Vector3 right;
    public ModGameObject modGameObj { get; set; }
    public int id { get; set; }
    public string playerName { get; set; }

    void Start()
    {
        modGameObj = new ModGameObject(this);
        go = GameObject.Find("Player");
        rb = go.GetComponent<Rigidbody>();
        jumpStartTime = 0.0f;
        jumpPhase = 0;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        UpdateDirection();
        UpdatePosition();
        DoJump();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Equals("Ground"))
        {
            jumpPhase = 0;
            jumpStartTime = 0f;
            nextJumpable = Time.time + 0.05f;
        }
    }

    void UpdateDirection()
    {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        var delta = transform.position - camera.transform.position;
        forward = Utils.Unitlize(new Vector3(delta.x, 0, delta.z));
        back = Vector3.Reflect(forward, forward);
        right = Utils.Unitlize(new Vector3(delta.z, 0f, -delta.x));
        left = Vector3.Reflect(right, right);
    }

    void UpdatePosition()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(forward*moveSpeed*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((jumpPhase < 2) && (nextJumpable < Time.time))
            {
                jumpPhase = jumpPhase + 1;
                jumpStartTime = Time.time;
                var pv = new Vector3(0.0f, premitiveV, 0.0f);
                rb.velocity = pv;
            }
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
}
