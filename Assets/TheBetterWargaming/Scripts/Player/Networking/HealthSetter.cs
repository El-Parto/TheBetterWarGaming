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
    [SyncVar] public bool playerJoined = false;

    [SyncVar] public int playerNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(playerJoined)
        {
            ServerGetPlayerList();
            
        }        
    }



    [ServerCallback]
    public void ServerGetPlayerList()
    {

        foreach(var player in playerList) // count each player GO in syncList
        {
            int id = -1;
            id++;
            // tell sync list which slider to give them
            // tell unity that this is the synclist and slider.
            giveThisSliderTo = playerHpSliders[id];
            playerList[id].GetComponent<NetworkPlayer>().hpSlider = giveThisSliderTo;
            
            
            //for(int i = 0; i < playerList.Count; i++) // for as long as i is less than player count
            //{

//          //}

            // reset player joined bool
            playerJoined = false;

            
        }

    }
    


}
