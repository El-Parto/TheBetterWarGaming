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
    
    // Start is called before the first frame update
    void Start()
    {
        cannon = GetComponentInChildren<Turret>().gameObject.transform;

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CmdFireBulletPrefab();            
        }
        
    }

    /// <summary>
    /// Gets the player reference so that it may be spawned in correctly with it's component.
    /// It's in a function because at the moment it's easier to remember than just calling OnStartClient with references
    /// to the component needed plus it's good Copy paste material.
    /// </summary>
    public void GetPlayerRef()
    {

    }

    public override void OnStartClient()
    {
        GetPlayerRef();
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

    
    [ClientRpc]
    public void RpcFireBulletPrefab()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newBullet = (Instantiate(bulletPrefab, cannon));
            NetworkServer.Spawn(newBullet);
            newBullet.transform.SetParent(null, true);
        }
            
        

    }


}
