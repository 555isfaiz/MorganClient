using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSButtonAction : MonoBehaviour
{
    void Start() {}

    void Update() {}

    public void OnClickStart()
    {
        MSMain.Login();
    }

    public void OnClickQuit()
    {
        MSMain.Quit();
    }
}