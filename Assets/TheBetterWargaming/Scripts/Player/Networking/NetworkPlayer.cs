using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(TankTEst))]
public class NetworkPlayer : NetworkBehaviour
{
    //SyncVar
    public GameObject bulletPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Gets the player reference so that it may be spawned in correctly with it's component.
    /// It's in a function because at the moment it's easier to remember than just calling OnStartClient with references
    /// to the component needed plus it's good Copy paste material.
    /// </summary>
    public void GetPlayerRef()
    {
        TankTEst playerTank = gameObject.GetComponent<TankTEst>();
        playerTank.enabled = isLocalPlayer;
    }

    public override void OnStartClient()
    {
        GetPlayerRef();
        // if we used Custom netowrk manager
        // Add player here.
    }

    // only if we had a custom network manager
    /*public override void OnStopClient()
    {
        // remove player here.
    }*/
    
    
    

    public void FireBulletPrefab()
    {
        GameObject newBullet = (Instantiate(bulletPrefab));
        if(Input.GetKeyDown(KeyCode.Space))
            Instantiate(bulletPrefab, cannon, false);

    }


}
