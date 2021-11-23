using UnityEngine;
using Mirror;

namespace Networking
{
    public class TankController : NetworkBehaviour
    {
        [Header("Setup")]
        [SerializeField] GameObject turret;
        [Header("Attributes")]
        [SerializeField] float tankSpeed = 2.50f; 
        [Tooltip("Rotation is based on degrees per second")]
        [SerializeField] float rotateSpeed = 215;

        void Update()
        {
            if (!CustomNetworkManager.Instance.canMove) return;
            if (!isLocalPlayer) return;
            MoveTank();
            RotateTurret();
        }

        // moves tank in direction using WASD
        void MoveTank()
        {
            gameObject.transform.position += new Vector3(tankSpeed * (Input.GetAxis("Horizontal") * Time.deltaTime), 0, tankSpeed * (Input.GetAxis("Vertical") * Time.deltaTime));
        }

        // rotates turret clockwise or counter clockwise
        void RotateTurret()
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKey(KeyCode.Q)) turret.transform.Rotate(0, 0, rotateSpeed * -1 * Time.deltaTime);
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKey(KeyCode.E)) turret.transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    } 
}
