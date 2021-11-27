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
        [Header("Setup")]
        [SerializeField] GameObject bulletPrefab;
        [SerializeField] Transform turret;
        [Header("SyncVars")]
        [SyncVar(hook = nameof(OnNameChanged))] public string playerName;
        [SyncVar(hook = nameof(OnColourChange))] public Color playerColour;
        [SyncVar(hook = nameof(OnHealthChange))] public float playerHealth = 100;
        [SyncVar(hook = nameof(OnAmmoChange))] public int playerAmmo = 3;
        [SyncVar] public float timeUntilRestockAmmo = 2;
        [SyncVar] public float timeUntilNextFire = 0.6f;
        [SyncVar] public bool canFire;
        [SyncVar] public float bulletDamage = 25.0f;
        [SyncVar] public float bulletLifetime = 4.0f;
        [SyncVar] public float bulletSpeed = 5.0f;
        GameObject[] players;
        [Header("UI")]
        [SerializeField] Slider healthSlider;
        [SerializeField] Slider ammoSlider;
        [SerializeField] TextMeshPro nameTag;
        public Material tankMaterial; // so that the player may choose their colour based off of the colour of the image. Ideally controlled by 3 other sliders. 
        [Header("Audio")]
        [SerializeField] AudioClip shootSound, engineSound;
        Scene currentScene;
        public static bool hasJoinedBefore;
        GameObject tempObject;
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
            RpcReturnToLobby();
            CmdReturnToLobby();
        }

        [Command]
        public void CmdReturnToLobby()
        {
            CustomNetworkManager.Instance.ServerChangeScene("Empty");
        }

        [ClientRpc]
        public void RpcReturnToLobby()
        {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
        }
        #endregion

        #endregion

        #region Overrides

        public override void OnStartLocalPlayer()
        {
            currentScene = SceneManager.GetActiveScene();
            if (!hasJoinedBefore) 
            {
                SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
                hasJoinedBefore = true;
            }

            // player name setup
            string name = PlayerNameInput.DisplayName;
            CmdChangeName(name);

            // player color setup
            Color color = new Color(ColourChangerUI.PlayerColour.r, ColourChangerUI.PlayerColour.g, ColourChangerUI.PlayerColour.b);
            CmdChangeColor(color);
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
            healthSlider = GetComponentInChildren<HealthUI>().gameObject.GetComponent<Slider>();
            ammoSlider = GetComponentInChildren<AmmoUI>().gameObject.GetComponent<Slider>();
        }

        void Start()
        {
            if (!isLocalPlayer) return;            
            SetupCursor();
        }

        // checks current scene to see if cursor needs to be locked or not
        void SetupCursor()
        {
            if (!currentScene.name.StartsWith("map")) UpdateCursor(CursorLockMode.None, true);
            else UpdateCursor(CursorLockMode.Locked, false);
        }

        // updates cursor lockmode and visibility
        void UpdateCursor(CursorLockMode lockState, bool visiblity)
        {
            Cursor.lockState = lockState;
            Cursor.visible = visiblity;
        }

        // player name setup
        [Command]
        public void CmdChangeName(string name) => playerName = name;
        void OnNameChanged(string _old, string _new) => nameTag.text = playerName;

        // player color setup
        [Command]
        void CmdChangeColor(Color color) => playerColour = color;
        void OnColourChange(Color _old, Color _new) => tankMaterial.color = playerColour; 

        void OnHealthChange(float _old, float _new)
        {
            playerHealth = _new;
            healthSlider.value = playerHealth;
            CmdDeath();
        }

        void OnAmmoChange(int _old, int _new)
        {
            playerAmmo = _new;
            ammoSlider.value = playerAmmo;
        }
        #endregion

        #region Operation

        [Client]
        void Update()
        {
            if (!isLocalPlayer) return;

            if (canFire)
            {
                if (Input.GetKeyDown(KeyCode.Space) && playerAmmo > 0)
                {
                    playerAmmo -= 1;
                    canFire = false;
                    CmdShoot();
                    SoundManager.Instance.PlaySound(shootSound);
                }
            }

            AmmoTeller();
        }

        // checks player health on server
        [Command]
        void CmdDeath()
        {
            if (playerHealth <= 0) RpcDeath();
        }

        // disables player for all observers
        [ClientRpc]
        void RpcDeath()
        {
            gameObject.SetActive(false);
            // add explosion particles
        }

        [Command]
        public void CmdShoot()
        {
            GameObject bullet = Instantiate(bulletPrefab, turret);
            bullet.transform.SetParent(null, true);
            bullet.GetComponent<Rigidbody>().velocity = turret.transform.forward * bulletSpeed;
            NetworkServer.Spawn(bullet);
        }

        // When ammo is 0, count down the timer , once timer is 0, reset ammo and timer values.
        // Also holds the timer for controlling the speed of how fast your tank fires
        public void AmmoTeller()
        {
            // if ammo is below 3, begin countdown till restocking ammo.
            if (playerAmmo <= 2)
            {
                timeUntilRestockAmmo -= 1 * Time.deltaTime;
                //Debug.Log($"timer = {timeUntilRestockAmmo}");
            }

            // if timer "expires" or hits 0 or below, add ammo. 
            if (timeUntilRestockAmmo <= 0)
            {
                playerAmmo++;
                timeUntilRestockAmmo = 2;
                //Debug.Log("Ammo refilled");
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
        void OnCollisionEnter(Collision other)
        {
            // if bullet hits player
            if (other.collider.CompareTag("Bullet"))
            {
                playerHealth -= bulletDamage;
                tempObject = other.gameObject;
                Destroy(tempObject.gameObject);
            }

            if (other.collider.CompareTag("DeathZone")) gameObject.transform.position = new Vector3(0, 3, 0);
        }
        #endregion
    }
}