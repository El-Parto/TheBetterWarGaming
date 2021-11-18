using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using UnityEngine.UI;

public class HealthSetter : NetworkBehaviour
{
    
    [SyncVar] public int id; // id given to the player
    public SyncList<GameObject> playerList = new SyncList<GameObject>(); // player is put into this list
    public Slider[] playerHpSliders = { };
    public Slider giveThisSliderTo;
    
    [SerializeField] private bool isActive = false;
    [SyncVar] public int playerNum = 0;
    [SyncVar] public bool canStartGame = false;
       
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() 
    {
    //     if(isActive)
    //     {
    //         SetHealth();       

    }



    // [Server]
    // public void ServerGetPlayerList()
    // {
    //     
    //
    //    if(playerJoined)
    //     {
    //         
    //         for(int i = 0; i < playerList.Count; i++)
    //         {
    //         
    //             //health slider
    //             GameObject hitpointSlider = Instantiate(playerHpSliders[i], GameObject.FindObjectOfType<Canvas>().transform, false).gameObject;
    //             NetworkServer.Spawn(hitpointSlider);
    //             
    //             // Transform spawmpos = GameObject.FindObjectOfType<Canvas>().transform;
    //             // GameObject ammoSliderOBJ = Instantiate(playerAmmoSliders[i], spawmpos, false).gameObject;
    //             // NetworkServer.Spawn(ammoSliderOBJ);
    //         
    //             // tell sync list which slider to give them
    //             // tell unity that this is the synclist and slider.
    //             giveThisSliderTo = playerHpSliders[i]; // this makes slider var = slider array[id] 
    //             playerList[i].GetComponent<NetworkPlayer>().hpSlider = hitpointSlider.GetComponent<Slider>(); // gives slider var to network player.hpslider
    //             playerList[i].GetComponent<NetworkPlayer>().hpSlider.value = playerList[i].GetComponent<NetworkPlayer>().health;
    //             //NetworkServer.Spawn(sliderObject);
    //             playerList[i].GetComponent<NetworkPlayer>().hpSlider.interactable = false;
    //         
    //             //ammo slider
    //             // giveThisSliderTo = playerAmmoSliders[i];
    //             // playerList[i].GetComponent<NetworkPlayer>().ammoSlider = giveThisSliderTo; // gives slider var to network player.hpslider
    //             //playerList[i].GetComponent<NetworkPlayer>().ammoSlider.interactable = false;
    //         
    //         
    //         
    //             // reset player joined bool so the loop doesn't reset.
    //             canStartGame = true;
    //             playerJoined = false;
    //             Debug.Log($"{playerList[i]}");
    //             
    //         
    //             // for some reason, upon getting the second player, the first value
    //             // of playerlist is NULL.
    //         }
    //     }
    //
    // }

    // [Server]
    // public void SetHealth()
    // {
    //     playerList[0].GetComponent<NetworkPlayer>().hpSlider.value = playerList[0].GetComponent<NetworkPlayer>().health;
    //     if(playerList[1]!= null)
    //         playerList[1].GetComponent<NetworkPlayer>().hpSlider.value = playerList[1].GetComponent<NetworkPlayer>().health;
    // }


}
