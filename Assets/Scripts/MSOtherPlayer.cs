using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSOtherPlayer : MonoBehaviour
{
    public ModMotion modMotion;
    public ModGameObject modGameObj {get; set;}
    public int playerId { get; set; }
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
        modGameObj.Update();
        modMotion.Update();
    }

    void OnCollisionEnter(Collision collision)
    {
        modMotion.OnCollisionEnter(collision);
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
