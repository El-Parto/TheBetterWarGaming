using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UISpawner : NetworkBehaviour
{
    [SerializeField] GameObject healthSlider1, healthSlider2;
    //[SerializeField] GameObject ammoSlider1, ammoSlider2;
    [SerializeField] Transform canvasTransform;
    public GameObject[] tempSliders;
    //public GameObject tempAmmoSlider1, tempAmmoSlider2;
    public int sliderCount = 0;

    [Server]
    void Awake()
    {
        


        // spawn health sliders
        GameObject healthSlider1Instance = Instantiate(healthSlider1, canvasTransform);
        GameObject healthSlider2Instance = Instantiate(healthSlider2, canvasTransform);
        NetworkServer.Spawn(healthSlider1Instance);
        NetworkServer.Spawn(healthSlider2Instance);
        tempSliders[0] = healthSlider1Instance;
        tempSliders[1] = healthSlider2Instance;

        // spawn ammo sliders
    }
}
