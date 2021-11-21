using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Bullet))]
public class NetworkBullet : NetworkBehaviour 
{
    [ServerCallback]
    public void OnCollisionEnter(Collision other)
    {
	    if(other.collider.CompareTag("Wall"))
	    {
		    Destroy(gameObject);
	    }
	    
    }
}
