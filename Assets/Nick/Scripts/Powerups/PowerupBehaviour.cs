using UnityEngine;
using Mirror;

namespace Powerup
{
    public class PowerupBehaviour : NetworkBehaviour
    {
        [SerializeField] Powerup powerup;

        [ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                powerup.Start();
                GameObject.Find("Powerups").GetComponent<PowerupSpawner>().isActive = false;
                Destroy(gameObject);
            }
        }

        public void SetPowerup(Powerup _powerup)
        {
            powerup = _powerup;
            gameObject.name = powerup.name;
        }
    } 
}
