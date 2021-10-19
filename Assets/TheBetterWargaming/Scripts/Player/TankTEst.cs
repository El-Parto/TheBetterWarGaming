using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTEst : MonoBehaviour
{

    public float tankSpeed;
    public float rotateSpeed;

    [SerializeField]private GameObject bulletPrefab;

    public GameObject turret;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveTank();
        RotateTurret();
        Shoot();
    }


    private void MoveTank()
    {
        gameObject.transform.position += new Vector3(tankSpeed * (Input.GetAxis("Horizontal")), 0, tankSpeed * (Input.GetAxis("Vertical")));
    }

    private void RotateTurret()
    {
        if(Input.GetKeyDown(KeyCode.Q)||Input.GetKey(KeyCode.Q))
            turret.transform.Rotate(0,rotateSpeed*-1,0);
        if(Input.GetKeyDown(KeyCode.E)|| Input.GetKey(KeyCode.E))
            turret.transform.Rotate(0,rotateSpeed,0);
    }

    private void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Instantiate(bulletPrefab, turret.transform, false);
        
    }

}
