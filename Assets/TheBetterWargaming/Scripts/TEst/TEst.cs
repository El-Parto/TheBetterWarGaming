using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class TEst : MonoBehaviour
{

    public float speed;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        gameObject.transform.Rotate(new Vector3(0,speed *Time.deltaTime,0));
    }
    
}
