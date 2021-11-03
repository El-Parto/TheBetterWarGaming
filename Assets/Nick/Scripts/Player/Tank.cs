using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    public float speed = 7.00f; // with current scale in mind, current speed = 0.01f
    public float rotateSpeed = 8.45f; // current rotate speed is 1.45f;
    [SerializeField] private GameObject bulletPrefab; // object instantiated when firing
    public GameObject turret;// object that is rotated via play control
    public Transform cannon;
    public float health = 100;
    public Slider healthSlider;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start() => rb = GetComponent<Rigidbody>();

    // Update is called once per frame
    void Update()
    {
        MoveTank();
        RotateTurret();
        //Shoot();
    }

    ///<summary> Moves tank in the specified direction via moving position with GetAxis. </summary>
    void MoveTank()
    {
        gameObject.transform.position += new Vector3(speed * (Input.GetAxis("Horizontal") * Time.deltaTime), 0, speed * (Input.GetAxis("Vertical") * Time.deltaTime));
    }
    ///<summary> Rotates the turret Clockwise or Counter Clockwise depending on if Q or E is pressed down. </summary>///
    void RotateTurret()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKey(KeyCode.Q))
            turret.transform.Rotate(0, rotateSpeed * -1 * Time.deltaTime, 0);
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E))
            turret.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
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