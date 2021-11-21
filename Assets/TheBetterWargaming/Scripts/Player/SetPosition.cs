using UnityEngine;

public class SetPosition : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(null,false);
        transform.position = Camera.main.transform.position;
    }
}
