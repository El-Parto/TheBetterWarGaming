using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

using System;

[RequireComponent(typeof(TankTEst))]
public class NetworkPlayer : NetworkBehaviour
{
    //SyncVar
    public GameObject bulletPrefab;
    public Transform cannon;
    [SyncVar] public int ammo = 3;
    [SyncVar] public float ammoTimer = 3;
    //[SyncVar] public bool noAmmo = false;
    
    // Start is called before the first frame update
    void Start()
    {
        cannon = GetComponentInChildren<Turret>().gameObject.transform;

    }

    
    public void Update()
    {
        if(hasAuthority)
        {
            if(Input.GetKeyDown(KeyCode.Space) && ammo != 0)
            {
                {
                    ammo -= 1;
                    CmdFireBulletPrefab();
                    
                }
            }
        }
        AmmoTeller();


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
        RpcFireBulletPrefab();
    }

    [ServerCallback]
    public void OnCollisionEnter(Collision other)
    {
        if(other.collider.CompareTag("Bullet"))
        {
            Rigidbody rbPlayer = gameObject.GetComponent<Rigidbody>();
            rbPlayer.AddForce(new Vector3(0,5,0), ForceMode.Impulse);
        }

        if(other.collider.CompareTag("DeathZone"))
        {
            gameObject.transform.position = new Vector3(0, 3, 0);
        }
        
    }
    [ClientRpc]
    public void RpcFireBulletPrefab()
    {
        GameObject newBullet = (Instantiate(bulletPrefab, cannon));
        newBullet.transform.SetParent(null, true);
        NetworkServer.Spawn(newBullet);
        
       
            
        

    }
/// <summary>
/// When ammo is 0, count down the timer , once timer is 0, reset ammo and timer values.
/// </summary>
    public void AmmoTeller()
    {
        
        if(ammo == 0)
        {

            ammoTimer -= 1 * Time.deltaTime; 
            Debug.Log($"timer = {ammoTimer}");
            
        }

        if(ammoTimer <= 0)
        {
            ammo = 3;
            ammoTimer = 3;
            Debug.Log("Ammo refilled");
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
