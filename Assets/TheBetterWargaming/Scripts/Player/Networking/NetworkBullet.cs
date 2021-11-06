using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using System;
using System.Security.Cryptography;

[RequireComponent(typeof(Bullet))]
public class NetworkBullet : NetworkBehaviour 
{
   // public void Awake()
    //{
     //   Bullet bullet = gameObject.GetComponent<Bullet>();
     //   bullet.enabled = isClient;
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ServerCallback]
    public void OnCollisionEnter(Collision other)
    {
	    if(other.collider.CompareTag("Wall"))
	    {
		    Destroy(gameObject);
	    }
	    
    }
}
