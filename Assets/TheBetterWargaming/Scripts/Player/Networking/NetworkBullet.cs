using UnityEngine;
using Mirror;

public class NetworkBullet : NetworkBehaviour 
{
    [ServerCallback]
    void OnCollisionEnter(Collision other)
    {
	    if(other.collider.CompareTag("Wall")) Destroy(gameObject);  
    }
}
