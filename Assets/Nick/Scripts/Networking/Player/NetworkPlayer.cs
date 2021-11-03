using UnityEngine;
using Mirror;

[RequireComponent(typeof(Tank))]
public class NetworkPlayer : NetworkBehaviour
{
    //SyncVar
    public GameObject bulletPrefab;
    public Transform cannon;
    [SyncVar] public int ammo = 3;
    [SyncVar] public float ammoTimer = 3;
    //[SyncVar] public bool noAmmo = false;

    void Start() => cannon = GetComponentInChildren<Turret>().gameObject.transform;

    public void Update()
    {
        if(isLocalPlayer)
        {
            if(Input.GetKeyDown(KeyCode.Space) && ammo > 0)
            {
                {
                    ammo -= 1;
                    CmdFireBulletPrefab();              
                }
            }
        }
        AmmoTeller();
    }

    /// <summary> Gets the player reference so that it may be spawned in correctly with it's component. </summary>
    public override void OnStartClient()
    {
        //GetPlayerRef();
        Tank playerTank = gameObject.GetComponent<Tank>();
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
        NetworkServer.Spawn(newBullet);
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
    public void RpcFireBulletPrefab(GameObject _bullet) => _bullet.transform.SetParent(null, true);

    /// <summary> When ammo is 0, count down the timer , once timer is 0, reset ammo and timer values. </summary>
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
