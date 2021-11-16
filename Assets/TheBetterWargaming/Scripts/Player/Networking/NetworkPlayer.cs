using JetBrains.Annotations;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using System;
using UnityEngine;
using UnityEngine.UI;

using UnityEngineInternal;

[RequireComponent(typeof(TankTEst))]
public class NetworkPlayer : NetworkBehaviour
{
    //Tank mechanic variables
    public GameObject bulletPrefab; // the bullet GO the player fires
    public Transform cannon; // the turret the player fires from
    [SyncVar] public int ammo = 3; // how much ammo the player has
    [SyncVar] public float ammoTimer = 2; // how longe before you restock ammo.
    [SyncVar] public float fireTimer = 0.6f; // how long until you can fire next bullet
    [SyncVar] private bool canFire; // fire check
    
    //Health variables and sliders

   public SyncList<GameObject> playerList = new SyncList<GameObject>();
   public Slider[] playerHpSliders = { };
   public Slider giveThisSliderTo; //slider Cache
   [SyncVar] public float health = 100;// health variable
   public Slider hpSlider; // the slider that the player gets wich is chosen based on the array of sliders below
   public Slider remotehpSliderP; // the slider that the player gets wich is chosen based on the array of sliders below
   // public Slider[] hpSliders =  {};

   [SyncVar] public bool playerJoined = false;
   [SerializeField] private GameObject sliderObject;
   
   // ammo sliders
   public Slider[] playerAmmoSliders;
   public Slider ammoSlider;
   
   // player id and bool
   private int pId;

   
   [SyncVar] public bool canStartGame = false;
   
   
   
   // color stooof
   // material sync list here
   // material cache here
   // actual material here
   // 
   
   
   
    [SyncVar] public bool isDead = false; // used to trigger set active to false;
    
   // public int playerID = 0; // i'd like this to be the actual id of the player but....
    [SerializeField] private bool isSet = false; // if this bool is true, it means the gui has been set (supposed to anyway)
    
    //[SyncVar] public bool noAmmo = false;
    

    /// <summary>
    /// Gets the player reference so that it may be spawned in correctly with it's component.
    /// </summary>
    public override void OnStartClient()
    {

        
        //GetPlayerRef();
        TankTEst playerTank = gameObject.GetComponent<TankTEst>();
        playerTank.enabled = isLocalPlayer;
        IDManager _id = GameObject.FindObjectOfType<IDManager>().GetComponent<IDManager>();
        
        pId = _id.playerCount;
        _id.playerCount++;


    }
    public override void OnStartLocalPlayer()
    {
        
        playerJoined = true;
        // if we used Custom netowrk manager
        // Add player here.
        Addplayer();
        CmdServerGetPlayerList();
        
            


    }

   

    [Server]
    public void Addplayer()
    {
        
        playerList.Add(gameObject);
    }

    void Start()
    { 
        
        cannon = GetComponentInChildren<Turret>().gameObject.transform;
        // if(isLocalPlayer)
        // {
        //     
        //         
        //     
        //    
        //     //setHealth.playerList.Add(gameObject); // add gameobect to the synclist
        //     // setHealth.playerJoined = true; // activates boolean
        //     // hpSlider = setHealth.playerHpSliders[iDs[playerID]]; // slider is now whatever the slider array is.    
        //     // isSet = true;
        //     // playerID++;// to set individual player ID's....don't think it's doing anything?
        // }
        // else if(!isLocalPlayer)
        // {
        //     hpSlider = setHealth.playerHpSliders[iDs[playerID]];
        // }
        //
            

    }


    public void Update()
    {

        if(isLocalPlayer)
        {
            

            //hpSlider.value = health;
            //ammoSlider.value = ammo;
            if(health <= 0)
                isDead = true;
            CmdOnDeath();
            //testing health
            if(Input.GetKeyDown(KeyCode.P))
            {
                health -= 25;
            }

            if(canFire)
            {
                if(Input.GetKeyDown(KeyCode.Space) && ammo > 0)
                {
                    {
                        ammo -= 1;
                        canFire = false;
                        CmdFireBulletPrefab();

                    }
                }
            }
        }


        // Ammo +cooldown mechanic
        AmmoTeller();
       

        CmdSetHealthAmmo();
        CheckPlayerStatus();
        
        //Death mechanic
        

    }
    
    
    // check if all players are ready
    [Server]
    public void CheckPlayerStatus()
    {
        IDManager _id = GameObject.FindObjectOfType<IDManager>().GetComponent<IDManager>();
        if(_id.playerCount >= 2)
        {
            canStartGame = true;
        }

        if(canStartGame)
        {
            //please replace with actual bool check and things
            Button startbutton = GameObject.FindObjectOfType<Button>().GetComponent<Button>();
            startbutton.interactable = true;
        }
            
            
    } 

    [Command]
    public void CmdSetHealthAmmo()
    {
        if(isLocalPlayer)
            playerList[0].GetComponent<NetworkPlayer>().hpSlider.value = health;
        RpcSetHealthAmmo();
    }
    [ClientRpc]
    [CanBeNull]
    public void RpcSetHealthAmmo()
    {
        IDManager idm = GameObject.FindObjectOfType<IDManager>().GetComponent<IDManager>();
        if(idm.playerCount >=2)
            playerList[1].GetComponent<NetworkPlayer>().remotehpSliderP.value = playerList[1].GetComponent<NetworkPlayer>().health;
            
            

    }



    [Command]
    public void CmdOnDeath()
    {
        RpcOnDeath();
    }

    /// <summary>
    /// Handles killing the player, it tells the server to destroy game object upon activating bool
    /// it's a little wonky how i've written it with the is dead check
    /// </summary>
    [ClientRpc]
    public void RpcOnDeath()
    {
        if(isDead)
        {
            if(health <= 0)
            {
                gameObject.SetActive(false); 
                // remove player command from here
                NetworkServer.Destroy(gameObject);
            }
        }
    }




    // public void OnHealthChange(float _old, float _new)
    // {
    //     hpSlider.value = health;
    // }

    [Command]
    public void CmdServerGetPlayerList()
    {
        RpcGetPlayerList();

    }

        

    

    [ClientRpc]
    public void RpcGetPlayerList()
    {
        IDManager idm = GameObject.FindObjectOfType<IDManager>().GetComponent<IDManager>();
        GameObject hitpointSlider = Instantiate(playerHpSliders[0], GameObject.FindObjectOfType<Canvas>().transform, false).gameObject;
        GameObject remoteHitpointSlider = Instantiate(playerHpSliders[1], GameObject.FindObjectOfType<Canvas>().transform, false).gameObject;
        NetworkServer.Spawn(hitpointSlider);
        NetworkServer.Spawn(remoteHitpointSlider);
        if(idm.playerCount >= 2)
        {
            remotehpSliderP = remoteHitpointSlider.GetComponent<Slider>();
            remotehpSliderP.value = playerList[1].GetComponent<NetworkPlayer>().health;
        }
        hpSlider = hitpointSlider.GetComponent<Slider>();

    }
    
   // [ClientRpc]
   //  public void RpcGetPlayerList(Slider _remoteSlider)
   //  {
   //      IDManager idm = GameObject.FindObjectOfType<IDManager>().GetComponent<IDManager>();
   //
   //      
   //
   //
   //      remotehpSliderP = remoteHitpointSlider.GetComponent<Slider>();
   //      
   //      if(isLocalPlayer )
   //      {
   //
   //      // playerList[0].GetComponent<NetworkPlayer>().hpSlider = playerHpSliders[0];
   //      // if(playerList[1] != null)
   //      //     playerList[1].GetComponent<NetworkPlayer>().hpSlider = playerHpSliders[1];
   //      // canStartGame 
   //
   //
   //          //health slider
   //
   //          
   //
   //          // giveThisSliderTo = playerHpSliders[pId]; // this makes slider var = slider array[id] 
   //          // playerList[pId].GetComponent<NetworkPlayer>().hpSlider = hitpointSlider.GetComponent<Slider>(); // gives slider var to network player.hpslider
   //          // playerList[pId].GetComponent<NetworkPlayer>().hpSlider.value = playerList[pId].GetComponent<NetworkPlayer>().health;
   //          // //NetworkServer.Spawn(sliderObject);
   //          // playerList[pId].GetComponent<NetworkPlayer>().hpSlider.interactable = false;
   //          //
   //          //ammo slider
   //          // giveThisSliderTo = playerAmmoSliders[i];
   //          // playerList[i].GetComponent<NetworkPlayer>().ammoSlider = giveThisSliderTo; // gives slider var to network player.hpslider
   //          //playerList[i].GetComponent<NetworkPlayer>().ammoSlider.interactable = false;
   //
   //      
   //          
   //          // Transform spawmpos = GameObject.FindObjectOfType<Canvas>().transform;
   //          // GameObject ammoSliderOBJ = Instantiate(playerAmmoSliders[i], spawmpos, false).gameObject;
   //          // NetworkServer.Spawn(ammoSliderOBJ);
   //          //
   //          // // tell sync list which slider to give them
   //          // // tell unity that this is the synclist and slider.
   //          // giveThisSliderTo = playerHpSliders[pId]; // this makes slider var = slider array[id] 
   //          // playerList[pId].GetComponent<NetworkPlayer>().hpSlider = hitpointSlider.GetComponent<Slider>(); // gives slider var to network player.hpslider
   //          // playerList[pId].GetComponent<NetworkPlayer>().hpSlider.value = playerList[pId].GetComponent<NetworkPlayer>().health;
   //          // //NetworkServer.Spawn(sliderObject);
   //          // playerList[pId].GetComponent<NetworkPlayer>().hpSlider.interactable = false;
   //          //
   //          // //ammo slider
   //          // // giveThisSliderTo = playerAmmoSliders[i];
   //          // // playerList[i].GetComponent<NetworkPlayer>().ammoSlider = giveThisSliderTo; // gives slider var to network player.hpslider
   //          // //playerList[i].GetComponent<NetworkPlayer>().ammoSlider.interactable = false;
   //          //
   //          //
   //          //
   //          // // reset player joined bool so the loop doesn't reset.
   //          if(playerList.Count >= 1)
   //          {
   //
   //
   //              canStartGame = true;
   //             
   //          }
   //
   //          //Debug.Log($"{playerList[pId]}");
   //          
   //      
   //          // for some reason, upon getting the second player, the first value
   //          // of playerlist is NULL.
   //          playerJoined = false;
   //      }
   //
   //  }



    // only if we had a custom network manager
    /*public override void OnStopClient()
    {
        // remove player here.
    }*/


    [Command]
    public void CmdFireBulletPrefab()
    {
        GameObject newBullet = (Instantiate(bulletPrefab, cannon));
        NetworkServer.Spawn(newBullet); // we don't add this to client rpc because NetworkServer.Spawn spawns on server and client rPC would also spawn the prefab causing double instantiation on the client.
        RpcFireBulletPrefab(newBullet);
    }

    [ClientRpc]
    public void RpcFireBulletPrefab(GameObject _bullet)
    {
        _bullet.transform.SetParent(null, true);

    }
/// <summary>
/// When ammo is 0, count down the timer , once timer is 0, reset ammo and timer values.
/// Also holds the timer for controlling the speed of how fast your tank fires
/// </summary>
    public void AmmoTeller()
    {
        // if ammo is below 3, begin countdown till restocking ammo.
        if(ammo <= 2)
        {

            ammoTimer -= 1 * Time.deltaTime; 
            Debug.Log($"timer = {ammoTimer}");
            
        }
        
        // if timer "expires" or hits 0 or below, add ammo. 
        if(ammoTimer <= 0)
        {
            ammo ++;
            ammoTimer = 2;
            Debug.Log("Ammo refilled");
        }

        if(!canFire)
        {
            fireTimer -= 1 * Time.deltaTime;
        }

        if(fireTimer <= 0)
        {
            canFire = true;
            fireTimer = 0.6f;
        }
        


    }

    [ServerCallback]
    public void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Bullet"))
        {
            Rigidbody rbPlayer = gameObject.GetComponent<Rigidbody>();
            rbPlayer.AddForce(new Vector3(0,5,0), ForceMode.Impulse);
            health -= 25;
            Destroy(other.gameObject);
        }

        if(other.collider.CompareTag("DeathZone"))
        {
            gameObject.transform.position = new Vector3(0, 3, 0);
        }
            
    }

/// <summary>
/// reduces the player count and removes from sync list.
/// </summary>
    // public override void OnStopClient()
    // {
    //     IDManager id = GameObject.FindObjectOfType<IDManager>().GetComponent<IDManager>();
    //     id.playerCount--;
    //     
    //     id.gameReady = false;
    //     playerList.Remove(gameObject);
    //     base.OnStopClient();
    //     
    // }

    /*
    public IEnumerator AmmoCooldown()
    {
        if(noAmmo)
        {
            yield return new WaitForSeconds(3);
            ammo = 3;
            noAmmo = false;

        }
        
    }
*/

}
