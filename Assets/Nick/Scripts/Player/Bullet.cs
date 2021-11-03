using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float travelSpeed = 3.46f;// currently set to 1.46f
    public Transform turret; // the game object that it will be fired from. or supposed to.
    //private GameObject kaboom; // this is for the explosion effect to be instantiated when the bullet destroys itself.
    Rigidbody brb;
    Tank tank;

    void Start()
    {
        // so the bullet appears at the cannon. OR is supposed to.
        gameObject.transform.position += new Vector3(0.055f, 0, 0);
        brb = GetComponent<Rigidbody>();
    }

    void Update() => StartCoroutine(BeABullet());

    public IEnumerator BeABullet()
    {
        gameObject.transform.SetParent(null);
        //gameObject.transform.rotation = new Quaternion(0, turret.transform.localRotation.y, 0, 0);
        gameObject.transform.Translate(travelSpeed * Time.deltaTime, 0, 0/*,turret.transform*/);

        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    private void Richochet()
    {
        //you take your aim, 
        // fire away, fire awayyyy.
        //RaycastHit wallBounce = Physics.Raycast()
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tank.health -= 25;
            Destroy(gameObject);
        }
    }
}