using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 3.46f;
    [SerializeField] float bulletLifetime;

    void Start() => Invoke("DestroyBullet", bulletLifetime);

    void Update() => MoveBullet();

    void MoveBullet() => transform.Translate(speed * Time.deltaTime, 0, 0);

    void DestroyBullet() => Destroy(gameObject);
}
