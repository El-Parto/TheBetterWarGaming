using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(null,false);
        transform.position = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
