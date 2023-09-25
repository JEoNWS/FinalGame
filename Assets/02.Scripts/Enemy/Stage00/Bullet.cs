using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    bool canMove = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!Player.GetInstance().playerCanHit)
                return;
            Player.GetInstance().Hit();
            GetComponent<Animator>().SetTrigger("Hit");
            canMove = false;
        }
    }
    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }
    public void DestrotThis()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if(canMove)
            transform.position += transform.right * speed * Time.deltaTime;
    }
}
