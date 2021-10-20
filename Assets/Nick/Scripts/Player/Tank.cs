using UnityEngine;

public class Tank : MonoBehaviour
{
    [Header("Attributes")]
    public float speed;
    [SerializeField] float rotateSpeed;
    [Space]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject turret;
    [Header("Controls")]
    [SerializeField] KeyCode rotateLeft;
    [SerializeField] KeyCode rotateRight;
    [SerializeField] KeyCode shoot;

    void Update()
    {
        MoveTank();
        RotateTurret();
        Shoot();
    }

    void MoveTank()
    {
        gameObject.transform.position += new Vector3(speed * (Input.GetAxis("Horizontal")), 0, speed * (Input.GetAxis("Vertical")));
    }

    void RotateTurret()
    {
        if(Input.GetKey(rotateLeft)) turret.transform.Rotate(0, rotateSpeed * -1, 0);
        if(Input.GetKey(rotateRight)) turret.transform.Rotate(0, rotateSpeed, 0);
    }

    void Shoot()
    {
        if(Input.GetKeyDown(shoot)) Instantiate(bulletPrefab, turret.transform, false);      
    }
}
