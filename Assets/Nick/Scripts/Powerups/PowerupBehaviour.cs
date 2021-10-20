using UnityEngine;

public class PowerupBehaviour : MonoBehaviour
{
    [SerializeField] Powerup powerup;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            powerup.Start();
            Destroy(gameObject);
        }
    }

    public void SetPowerup(Powerup _powerup)
    {
        powerup = _powerup;
        gameObject.name = powerup.name;
    }
}
