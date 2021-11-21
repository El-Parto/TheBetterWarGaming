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
        [Header("Attributes")]
        [SyncVar(hook = nameof(OnNameChanged))] public string playerName;
        [SyncVar (hook = nameof(OnColourChange)), SerializeField] Color playerColour;
        [SyncVar(hook = nameof(OnHealthChange))] public float tankHealth = 100;                                                
        [SyncVar] public float timeUntilRestockAmmo = 2;                                               
        [SyncVar] public float timeUntilNextFire = 0.6f;
        [SyncVar(hook = nameof(OnAmmoChange))] public int ammoAmount = 3;                                                     
        [SyncVar] public bool canFire;
        GameObject[] players;
        [Header("UI")]
        [SerializeField] Slider healthSlider;
        [SerializeField] Slider ammoSlider;
        [SerializeField] TextMeshPro nameTag;
        public Material tankMaterial; // so that the player may choose their colour based off of the colour of the image. Ideally controlled by 3 other sliders. 
        [Header("Audio")]
        [SerializeField] AudioClip shootSound, engineSound;
        Scene currentScene;
        public static bool hasJoinedForFirstTime;
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
        public void CmdReturnToLobby()
        {
            CustomNetworkManager.Instance.ServerChangeScene("Empty");
            RpcReturnToLobby();
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
            if (!hasJoinedForFirstTime) LoadLobbyAdditively();
            SetupPlayer();
        }

        public void LoadLobbyAdditively()
        {
            SceneManager.LoadScene("Lobby", LoadSceneMode.Additive);
            hasJoinedForFirstTime = true;
        }

        public void SetupPlayer()
        {
            string name = PlayerNameInput.DisplayName;
            CmdChangeName(name);

            Color _playerColour = new Color(ColourChangerUI.PlayerColour.r, ColourChangerUI.PlayerColour.g, ColourChangerUI.PlayerColour.b);
            CmdChangeColor(_playerColour);
        }

        // called when client or host connects
        public override void OnStartClient()
        {
            CustomNetworkManager.AddPlayer(this);
        }

        // called when client or host disconnects
        public override void OnStopClient()
        {
            CustomNetworkManager.RemovePlayer(this);
        }

        #endregion

        #region Setup

        void Awake() => currentScene = SceneManager.GetActiveScene();

        void Start()
        {
            if (!isLocalPlayer) return;            
            SetupCursor();
            CmdSetupSliders();
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

        [Command]
        public void CmdSetupSliders()
        {
            healthSlider = GetComponentInChildren<HealthUI>().gameObject.GetComponent<Slider>();
            ammoSlider = GetComponentInChildren<AmmoUI>().gameObject.GetComponent<Slider>();
        }

        void OnHealthChange(float _old, float _new)
        {
            tankHealth = _new;
            healthSlider.value = tankHealth;
        }

        void OnAmmoChange(int _old, int _new)
        {
            ammoAmount = _new;
            ammoSlider.value = ammoAmount;
        }
        #endregion

        #region Operation

        void Update()
        {
            if (!isLocalPlayer) return;

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
            CmdDeath();
        }

        [Command]
        void CmdDeath()
        {
            if (tankHealth <= 0) gameObject.SetActive(false);
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
            }

            if (other.collider.CompareTag("DeathZone")) gameObject.transform.position = new Vector3(0, 3, 0);
        }

        #endregion
    }
}