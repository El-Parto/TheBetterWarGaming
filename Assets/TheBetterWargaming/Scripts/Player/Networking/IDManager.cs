using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using UnityEngine.UI;

public class IDManager : NetworkBehaviour
{
    [SyncVar] public int playerCount = 0;
    [SyncVar] public bool gameReady = false;

    [SyncVar] public bool gameActive = false;
    public SyncList<GameObject> playerList = new SyncList<GameObject>();
    // this script is to count the player so that the game knows when to start
    // Player count is assigned to each player upon being instantiated, then the player
    // tells this script to increase by 1.
    // Because we''ve limited the player count to two,
    // player count will never count over 2 because assuming the server is still running, player 1 should always have 0 as it's assigned int.
    // and because of this, 0 will be the number used to place it in the sync list
    // and 1 will be used for player 2 in the sync list.

    // Update is called once per frame
    void Update()
    {
        // 
        if(playerCount >= 3)
            playerCount = 2;
        if(playerCount >= 2)
            gameReady = true;
        // i'm sorry about all the ifs
        
    }

    [Server]
    public void OnStartGame()
    {
        Button button = GameObject.FindObjectOfType<Button>().GetComponent<Button>();
        button.gameObject.SetActive(false);
        gameActive = true;
        
    } 
    

}
