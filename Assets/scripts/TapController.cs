using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapController : MonoBehaviour
{
    public bool isOn;

    public void TapOn()
    {
        if(!isOn)
        {
            isOn = true;
            Debug.Log("Tap is on...");
        }
    }
}
