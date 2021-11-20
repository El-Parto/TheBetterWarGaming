using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Networking
{
    [RequireComponent(typeof(TankController))]
    public class NetworkPlayer : NetworkBehaviour
    {
        #region Variables
        UISpawner uiSpawner;
        [Header("Setup")]
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] Transform turret;                                                            
        [Header("Attributes")]
        [SyncVar(hook = nameof(OnNameChanged))] public string playerName;
        [SyncVar] public float tankHealth = 100;                                                
        [SyncVar] public float timeUntilRestockAmmo = 2;                                               
        [SyncVar] public float timeUntilNextFire = 0.6f;                                            
        [SyncVar] public int ammoAmount = 3;                                                     
        [SyncVar] public bool canFire;
        GameObject[] players;
        [Header("UI")]
        [SerializeField] Slider healthSlider;
        [SerializeField] Slider ammoSlider;
        [SerializeField] TextMeshPro nameTag;
        [Header("Audio")]
        [SerializeField] AudioClip shootSound, engineSound;
        Scene currentScene;
        #endregion

        #region Lobby

        #region Starting Game
        public void StartMatch()
        {
            if (!isLocalPlayer) return;
            CmdStartMatch();
        }

        [Command]
        public void CmdStartMatch() => MatchManager.instance.StartMatch();
        #endregion

        #region Disconnecting
        public void LeaveMatch()
        {
            if (!isLocalPlayer) return;
            CmdLeaveMatch();
        }

        [Client]
        public void CmdLeaveMatch()
        {
            CustomNetworkManager.Instance.StopClient();
            if (isServer) CustomNetworkManager.Instance.StopHost();
            SceneManager.LoadScene("Menu");
        }
        #endregion

        #region Readying
        public void Ready()
        {
            if (!isLocalPlayer) return;
            CmdReady();
        }

        [Command]
        public void CmdReady()
        {

        }
        #endregion

        #region Results
        public void ReturnToLobby()
        {
            if (!isLocalPlayer) return;
            CmdReturnToLobby();
        }

        [Command]
        public void CmdReturnToLobby() => CustomNetworkManager.Instance.ServerChangeScene("Lobby");
        #endregion

        #endregion

        #region Overrides

        public override void OnStartLocalPlayer()
        {           
            if (!currentScene.name.StartsWith("map")) SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
            turret = GetComponentInChildren<Turret>().transform;
            string name = PlayerNameInput.DisplayName;
            CmdPlayerName(name);
        }

        // called when client or host connects
        public override void OnStartClient() => CustomNetworkManager.AddPlayer(this);

        // called when client or host disconnects
        public override void OnStopClient() => CustomNetworkManager.RemovePlayer(this);

        #endregion

        #region Setup

        void Awake()
        {
            currentScene = SceneManager.GetActiveScene();
            if (currentScene.name.StartsWith("map") && currentScene.name != "map_Results") uiSpawner = FindObjectOfType<UISpawner>();
        }

        void Start()
        {
            if (currentScene.name == "Lobby" || currentScene.name == "Empty" || currentScene.name == "map_Results")
            {
                UpdateCursor(CursorLockMode.None, true);
                return;
            }

            if (!isLocalPlayer) return;

            SetupTankUI();
        }

        // health and ammo UI setup
        void SetupTankUI()
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                if (healthSlider == null)
                {
                    player.GetComponent<NetworkPlayer>().healthSlider = uiSpawner.tempSliders[uiSpawner.sliderCount].GetComponent<Slider>();
                    uiSpawner.sliderCount += 1;
                }
            }

            //healthSlider1.GetComponent<Slider>().value = tankHealth;
            //ammoSlider.GetComponent<Slider>().value = ammoAmount;
        }

        // player info sent to server, then server updates sync vars which handles it on all clients
        [Command]
        public void CmdPlayerName(string name)
        {
            playerName = name;
        }

        void OnNameChanged(string _old, string _new) => nameTag.text = playerName;

        // updates cursor lockmode and visibility
        void UpdateCursor(CursorLockMode mode, bool visible)
        {
            Cursor.lockState = mode;
            Cursor.visible = visible;
        }
        #endregion

        #region Operation

        void Update()
        {
            if (currentScene.name == "Lobby" || currentScene.name == "Empty" || currentScene.name == "map_Results")
            {
                return;
            }

            if (!isLocalPlayer) return;

            // health test
            if (Input.GetKeyDown(KeyCode.P)) tankHealth -= 25;

            if (canFire)
            {
                if (Input.GetKeyDown(KeyCode.Space) && ammoAmount > 0)
                {
                    ammoAmount -= 1;
                    canFire = false;
                    CmdFireBullet();
                    SoundManager.Instance.PlaySound(shootSound);
                }
            }
            // Ammo +cooldown mechanic
            AmmoTeller();

            CmdOnDeath(); 
        }

        // if this player's health is 0, this player's GameObject is set to inactive
        [Command]
        public void CmdOnDeath()
        {
            if (tankHealth > 0) return;
            gameObject.SetActive(false);
        }

        // NetworkServer.Spawn needs to be called on the server
        [Command]
        public void CmdFireBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, turret);
            NetworkServer.Spawn(bullet); 
            RpcFireBullet(bullet);
        }

        [ClientRpc]
        public void RpcFireBullet(GameObject _bullet) => _bullet.transform.SetParent(null, true);

        // When ammo is 0, count down the timer , once timer is 0, reset ammo and timer values.
        // Also holds the timer for controlling the speed of how fast your tank fires
        public void AmmoTeller()
        {
            // if ammo is below 3, begin countdown till restocking ammo.
            if (ammoAmount <= 2)
            {
                timeUntilRestockAmmo -= 1 * Time.deltaTime;
                Debug.Log($"timer = {timeUntilRestockAmmo}");
            }

            // if timer "expires" or hits 0 or below, add ammo. 
            if (timeUntilRestockAmmo <= 0)
            {
                ammoAmount++;
                timeUntilRestockAmmo = 2;
                Debug.Log("Ammo refilled");
            }

            if (!canFire)
            {
                timeUntilNextFire -= 1 * Time.deltaTime;
            }

            if (timeUntilNextFire <= 0)
            {
                canFire = true;
                timeUntilNextFire = 0.6f;
            }
        }

        // server handles collision
        [ServerCallback]
        void OnCollisionEnter(Collision other)
        {
            // if bullet hits player
            if (other.collider.CompareTag("Bullet"))
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
                tankHealth -= 25;
                Destroy(other.gameObject);
            }

            if (other.collider.CompareTag("DeathZone")) gameObject.transform.position = new Vector3(0, 3, 0);
        }

        #endregion
    }
}