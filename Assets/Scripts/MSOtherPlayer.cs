using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSOtherPlayer : MonoBehaviour
{
    public ModGameObject modGameObj {get; set;}
    public int playerId { get; set; }
    public string playerName { get; set; }

    void Start()
    {
        modGameObj = new ModGameObject(this);
        modGameObj.Start();
    }

    void Update()
    {
        modGameObj.Update();
    }
}
