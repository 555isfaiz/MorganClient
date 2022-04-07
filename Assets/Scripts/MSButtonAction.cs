using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSButtonAction : MonoBehaviour
{
    void Start() {}

    void Update() {}

    public void OnClickShooter()
    {
        Debug.Log("click shooter!");
        MSMain.OnClickShooter();
    }

    public void OnClickTarget()
    {
        Debug.Log("click target!");
        MSMain.OnClickTarget();
    }
}