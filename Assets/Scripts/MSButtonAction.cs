using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSButtonAction : MonoBehaviour
{
    void Start() {}

    void Update() {}

    public void OnClickShooter()
    {
        MSShare.OnClickShooter();
    }

    public void OnClickTarget()
    {
        MSShare.OnClickTarget();
    }
}