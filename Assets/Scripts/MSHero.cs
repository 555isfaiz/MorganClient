using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MSHero : MonoBehaviour
{
    public ModMotion modMotion;    
    public ModWeapon modWeapon;
    public ModGameObject modGameObj { get; set; }
    public int id { get; set; }
    public string playerName { get; set; }

    void Start()
    {
        modGameObj = new ModGameObject(this);
        modMotion = new ModMotion(this, gameObject);
        modWeapon = new ModWeapon(this, gameObject);
        modGameObj.Start();
        modMotion.Start();
        modWeapon.Start();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        modMotion.Update();
        modWeapon.Update();
    }

    void OnCollisionEnter(Collision collision)
    {
        modMotion.OnCollisionEnter(collision);
        modGameObj.OnCollisionEnter(collision);
    }
}
