using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CanvasLookAtCamera : MonoBehaviour
{
    public RotationConstraint rCon;

    public ConstraintSource src;
    private void Start()
    {
        //src = Camera.main.transform ;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(Camera.main.transform, -Vector3.up);
        
    }
}
