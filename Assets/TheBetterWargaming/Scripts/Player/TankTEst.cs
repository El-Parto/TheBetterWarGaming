using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.UI;

public class TankTEst : MonoBehaviour
{

    public float tankSpeed = 2.50f; // Movement speed of tank. Only inspector value is loaded on runtime
    public float rotateSpeed = 215; // Rotation based on degrees per second. currently set to 145.

    [SerializeField]private GameObject bulletPrefab; // object instantiated when firing

    public GameObject turret;// object that is rotated via play control

    public Transform cannon;

    public float health = 100;
    public Slider healthSlider; 

    public Rigidbody rb;
  
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTank();
        RotateTurret();
        //Shoot();
    }

///<summary> Moves tank in the specified direction via moving position with GetAxis. </summary>
    private void MoveTank()
    {
        gameObject.transform.position += new Vector3(tankSpeed * (Input.GetAxis("Horizontal") * Time.deltaTime), 0, tankSpeed * (Input.GetAxis("Vertical")* Time.deltaTime));
    }
    ///<summary> Rotates the turret Clockwise or Counter Clockwise depending on if Q or E is pressed down. </summary>///
    private void RotateTurret()
    {
        if(Input.GetKeyDown(KeyCode.Q)||Input.GetKey(KeyCode.Q))
            turret.transform.Rotate(0,rotateSpeed*-1 * Time.deltaTime,0);
        if(Input.GetKeyDown(KeyCode.E)|| Input.GetKey(KeyCode.E))
            turret.transform.Rotate(0,rotateSpeed * Time.deltaTime,0);
    }
/// <summary>
/// When the player presses space, shoot the designated bullet prefab. Now handled on the Network Tank. or Networkplayer.
/// </summary>
    // private void Shoot()
    // {
    //     if(Input.GetKeyDown(KeyCode.Space))
    //         Instantiate(bulletPrefab, cannon, false);
    //     
    // }

    public void Damage()
    {
        // this would go into a on collision method
        
        
    }


}
