using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Move());
    }
    public IEnumerator Move()
    {
        float currentSpeed = speed;
        gameObject.transform.Translate(new Vector3(0,0,speed*Time.deltaTime));
        yield return new WaitForSeconds(5);
        speed = 0;
        yield return new WaitForSeconds(5);
        speed = currentSpeed * -1;
        yield return new WaitForSeconds(5);
        speed = 0;
        yield return new WaitForSeconds(5);
        speed = currentSpeed;
    }

}
