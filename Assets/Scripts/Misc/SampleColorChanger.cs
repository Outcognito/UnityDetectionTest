using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleColorChanger : MonoBehaviour
{
    private bool rChange = false;
    public void Green()
    {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }
    public void Yellow()
    {
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
    public void Red()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public void SwitchColor()
    {
        rChange = true;
    }
    private void Update()
    {
        if (rChange)
        {
            Red();
            rChange = false;
        }
        else Green();
    }
}
