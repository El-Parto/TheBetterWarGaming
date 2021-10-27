using UnityEngine;

namespace Powerup
{
    public class PowerupActions : MonoBehaviour
    {
        [SerializeField] Tank tank;
        [SerializeField] float speedBoost;
        [SerializeField] float duration;

        #region Powerups
        public void StartSpeedBoost()
        {
            tank.speed += speedBoost;
            Invoke("StopSpeedBoost", duration);
        }

        public void StopSpeedBoost()
        {
            tank.speed -= speedBoost;
        }

        #endregion
    } 
}
