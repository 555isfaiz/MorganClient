using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MSHero : MonoBehaviour
{
    public ModMotion modMotion;    
    public ModGameObject modGameObj { get; set; }
    public int id { get; set; }
    public string playerName { get; set; }

    void Start()
    {
        modGameObj = new ModGameObject(this);
        modMotion = new ModMotion(this, gameObject);
        modGameObj.Start();
        modMotion.Start();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        modMotion.Update();
    }

    void OnCollisionEnter(Collision collision)
    {
        modMotion.OnCollisionEnter(collision);
    }
}
