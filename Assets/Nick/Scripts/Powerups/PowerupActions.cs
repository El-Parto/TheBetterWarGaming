using UnityEngine;
using Mirror;

namespace Powerup
{
    public class PowerupActions : NetworkBehaviour
    {
        Tank tank;
        [SerializeField] float speedBoost;
        [SerializeField] float duration;

        private void Awake() => tank = GetComponent<Tank>();

        #region Powerups       
        public void StartSpeedBoost()
        {
            if (!isLocalPlayer) return;

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
