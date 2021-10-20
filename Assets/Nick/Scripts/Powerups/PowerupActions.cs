using UnityEngine;

public class PowerupActions : MonoBehaviour
{
    [SerializeField] Tank tank;
    [SerializeField] float speedBoost;
    [SerializeField] float duration;

    public void StartSpeedBoost()
    {
        tank.speed += speedBoost;
        Invoke("StopSpeedBoost", duration);
    }

    public void StopSpeedBoost()
    {
        tank.speed -= speedBoost;
    }
}
