using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void SwitchScene(string sceneName) => SceneManager.LoadScene(sceneName);
}
