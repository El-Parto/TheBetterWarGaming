using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // a bool on the player is set to true when this is picked up (hasFlamethrower = true)
        // the player can use this power once and then the bool is set to false (hasFlamethrower = false)
    }
}
