using Mirror;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Networking
{
    public delegate void SceneLoadedDelegate(Scene scene);

    public class NetworkSceneManager : NetworkBehaviour
    {
        public void LoadNetworkScene(string scene)
        {
            if (isLocalPlayer) CmdLoadNetworkScene(scene);
        }

        [Command]
        public void CmdLoadNetworkScene(string scene) => RpcLoadNetworkScene(scene);

        [ClientRpc]
        public void RpcLoadNetworkScene(string scene) => LoadScene(scene, loadedScene => SceneManager.SetActiveScene(loadedScene));

        public void LoadScene(string sceneName, SceneLoadedDelegate onSceneLoaded = null) => StartCoroutine(LoadScene_CR(sceneName, onSceneLoaded));

        IEnumerator LoadScene_CR(string sceneName, SceneLoadedDelegate onSceneLoaded = null)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            onSceneLoaded?.Invoke(SceneManager.GetSceneByName(sceneName));
        }
    } 
}
