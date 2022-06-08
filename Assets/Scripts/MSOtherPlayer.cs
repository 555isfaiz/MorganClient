using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSOtherPlayer : MonoBehaviour
{
    public ModMotion modMotion;
    public ModWeapon modWeapon;
    public ModGameObject modGameObj {get; set;}
    public int playerId { get; set; }
    public string playerName { get; set; }
    public bool acceptSync { get; set; }

    void Start()
    {
        acceptSync = true;
        modGameObj = new ModGameObject(this);
        modMotion = new ModMotion(this, gameObject);
        modWeapon = new ModWeapon(this, gameObject);
        modGameObj.Start();
        modMotion.Start();
        modWeapon.Start();
    }

    void Update()
    {
        modGameObj.Update();
        modMotion.Update();
        modWeapon.Update();
    }

    void OnCollisionEnter(Collision collision)
    {
        modMotion.OnCollisionEnter(collision);
        modGameObj.OnCollisionEnter(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        modMotion.OnCollisionExit(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        modMotion.OnCollisionStay(collision);
    }
}
