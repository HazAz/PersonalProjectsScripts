using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorReseter : MonoBehaviour
{
    public static ColorReseter Instance { set; get; }

    public Material blueMat;
    public Material redMat;
    public Material greyMat;
    
    void Start()
    {
        Instance = this;
    }
    
    public void SetGrey(Units u)
    {
        u.GetComponent<Renderer>().material = greyMat;
    }

    public void ResetColor(Units u)
    {
        u.GetComponent<Renderer>().material = (u.isRed) ? redMat : blueMat;
    }
}
