using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float travelSpeed;

    public GameObject turret;
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
        //gameObject.transform.SetParent(null, false);
            //gameObject.transform.rotation = new Quaternion(0, turret.transform.localRotation.y, 0, 0);
        gameObject.transform.position+= Vector3.right * travelSpeed * Time.deltaTime;
        
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    
    
}
