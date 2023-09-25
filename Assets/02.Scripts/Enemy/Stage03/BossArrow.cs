using UnityEngine;

public class BossArrow : MonoBehaviour
{
    [SerializeField]
    float speed;
    public GameObject dust;
    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Player.GetInstance().Hit();
            DestroyAnim();
        }
        else if(other.CompareTag("Ground"))
            DestroyGroundAnim(other.transform);
    }
    void DestroyAnim()
    {
        Instantiate(dust, transform.position, Quaternion.Euler(0.0f, 0.0f, transform.rotation.z + 45.0f));
        Destroy(gameObject);
    }
    void DestroyGroundAnim(Transform ground)
    {
        Instantiate(dust, new Vector3(transform.position.x, 0.0f, transform.position.z), Quaternion.Euler(0.0f, 0.0f, transform.rotation.z + 45.0f), ground);
        Destroy(gameObject);
    }
}
