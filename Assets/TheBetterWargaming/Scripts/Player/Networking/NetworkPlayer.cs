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
    [SyncVar] public float health = 100;// health variable
    public Slider hpSlider; // the slider that the player gets wich is chosen based on the array of sliders below
   // public Slider[] hpSliders =  {};

    [SyncVar] public bool isDead = false; // used to trigger set active to false;
    public SyncList<int> iDs = new SyncList<int>(); // this is the list used so that the player knows which slider to use.
    public int playerID = -1; // i'd like this to be the actual id of the player but....
    [SerializeField] private bool isSet = false; // if this bool is true, it means the gui has been set (supposed to anyway)
    
    //[SyncVar] public bool noAmmo = false;
    
    // Start is called before the first frame update
    public override void OnStartLocalPlayer()
    {

        playerID++;// to set individual player ID's....don't think it's doing anything?
        HealthSetter setHealth= FindObjectOfType<HealthSetter>();//  // gets healthsetter script so it can reference it below.
        if(!isSet)
        {
            setHealth.playerList.Add(gameObject); // add gameobect to the synclist
            setHealth.playerJoined = true; // activates boolean
            hpSlider = setHealth.playerHpSliders[iDs[playerID]]; // slider is now whatever the slider array is.    
            isSet = true;
        }
        

    }

    void Start()
    {
        cannon = GetComponentInChildren<Turret>().gameObject.transform;
        if(isLocalPlayer)
        {

            

        }
        else if(!isLocalPlayer)
        {
            
        }
        
            

    }


    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            HealthSetter hp = FindObjectOfType<HealthSetter>();
            hp.playerJoined = true;
        }
        if(isLocalPlayer)
        {
            
            hpSlider.value = health;
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
        
        //Death mechanic
        
        SetHealth();
        
        


    }

    public void SetHealth()
    {
        if(isLocalPlayer)
        {
            hpSlider.value = health;
        }
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





    /// <summary>
    /// Gets the player reference so that it may be spawned in correctly with it's component.
    /// </summary>
    public override void OnStartClient()
    {
        //GetPlayerRef();
        TankTEst playerTank = gameObject.GetComponent<TankTEst>();
        playerTank.enabled = isLocalPlayer;
        // if we used Custom netowrk manager
        // Add player here.
    }

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
