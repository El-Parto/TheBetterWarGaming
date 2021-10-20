using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float travelSpeed;

    public GameObject turret;
    //private GameObject kaboom; // this is for the explosion effect to be instantiated when the bullet destroys itself.
    
    // Start is called before the first frame update
    void Start()
    {
        
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
        gameObject.transform.Translate( 0, travelSpeed * Time.deltaTime,0);
        
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void Richochet()
    {
        //you take your aim, 
        // fire away, fire awayyyy.
        //RaycastHit wallBounce = Physics.Raycast()
        
    }
    
    
}
