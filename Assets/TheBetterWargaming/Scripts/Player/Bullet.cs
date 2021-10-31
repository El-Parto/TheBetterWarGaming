using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float travelSpeed;// currently set to 1.46f

    public Transform turret; // the game object that it will be fired from. or supposed to.
    //private GameObject kaboom; // this is for the explosion effect to be instantiated when the bullet destroys itself.

    private Rigidbody brb;
    private TankTEst tank;
    
    // Start is called before the first frame update
    void Start()
    {
        // so the bullet appears at the cannon. OR is supposed to.
        gameObject.transform.position += new Vector3(0.055f,0,0);

        brb = GetComponent<Rigidbody>();
        

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(BeABullet());
    }

    public IEnumerator BeABullet()
    {
        gameObject.transform.SetParent(null);
            //gameObject.transform.rotation = new Quaternion(0, turret.transform.localRotation.y, 0, 0);
        gameObject.transform.Translate( travelSpeed * Time.deltaTime, 0, 0/*,turret.transform*/);
        
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void Richochet()
    {
        //you take your aim, 
        // fire away, fire awayyyy.
        //RaycastHit wallBounce = Physics.Raycast()
        
        
    }

    public void OnCollisionrEnter(Collider _collision)
    {
        if(_collision.CompareTag("Player"))
        {
            tank.health -= 25;
            Destroy(gameObject);
        }
    }
}
