using UnityEngine;
using System.Collections.Generic;
using Mirror;

namespace Powerup
{
    public class PowerupSpawner : NetworkBehaviour
    {
        [SerializeField] List<Powerup> powerups;
        [SerializeField] List<GameObject> powers = new List<GameObject>();
        [SerializeField] List<Transform> points = new List<Transform>();
        [Tooltip("The duration before another powerup is spawned")]
        [SerializeField] float delay;
        public bool isActive;
        float timer;
        int powerIndex;
        int pointIndex;

        void Update()
        {
            if (isActive) return;
            Timer();
        }

        // a powerup is spawned every few seconds
        [ServerCallback]
        void Timer()
        {
            if (timer > delay)
            {
                ChooseRandomPoint();
                ChooseRandomPower();
                SpawnRandomPower();
                timer = 0;
            }
            else timer += Time.deltaTime;
        }


        // chooses random spawnpoint from list
        [ServerCallback]
        void ChooseRandomPoint() => pointIndex = Random.Range(0, points.Count);

        // chooses random powerup from list
        [ServerCallback]
        void ChooseRandomPower() => powerIndex = Random.Range(0, powers.Count);

        // spawns random powerup at random spawnpoint
        [ServerCallback]
        void SpawnRandomPower()
        {
            GameObject a = Instantiate(powers[powerIndex], transform);
            a.transform.position = points[pointIndex].position;
            var b = a.GetComponent<PowerupBehaviour>();
            b.SetPowerup(powerups[Random.Range(0, powerups.Count)]);
            isActive = true;
        }
    } 
}
