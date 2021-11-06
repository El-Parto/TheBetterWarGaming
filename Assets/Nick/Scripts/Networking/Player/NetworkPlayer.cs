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
    [SyncVar] public float ammoTimer = 2;
    [SyncVar] public float fireTimer = 0.6f;
    [SyncVar] public bool canFire;
    [SyncVar] public float health = 100;
    //[SyncVar] public bool noAmmo = false;
    
    // Start is called before the first frame update
    void Start()
    {
        cannon = GetComponentInChildren<Turret>().gameObject.transform;

    }

    
    public void Update()
    {
        if(isLocalPlayer)
        {
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
        GameObject newBullet = (Instantiate(bulletPrefab, cannon));
        NetworkServer.Spawn(newBullet); // we don't add this to client rpc because NetworkServer.Spawn spawns on server and client rPC would also spawn the prefab causing double instantiation on the client.
        RpcFireBulletPrefab(newBullet);
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
