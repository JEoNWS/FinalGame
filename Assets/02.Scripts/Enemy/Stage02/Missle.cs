using UnityEngine;

public class Missle : MonoBehaviour
{
    [SerializeField]
    float yMin;
    [SerializeField]
    float speed;
    Vector3 targetPos;
    bool hit;
    void Start()
    {
        hit = false;
        targetPos = Player.GetInstance().transform.position - transform.position;
    }

    void Update()
    {
        if (!hit)
        {
            if (transform.position.y > yMin)
            {
                targetPos = Player.GetInstance().transform.position - transform.position;
                transform.right = Vector3.MoveTowards(transform.right, targetPos, 0.1f);
            }
            transform.position += transform.right * Time.deltaTime * speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Crash();
            Player.GetInstance().Hit();
        }
        else if(collision.CompareTag("Ground"))
        {
            Crash();
        }
        else if(collision.CompareTag("Missle"))
        {
            Crash();
            collision.GetComponent<Missle>().Crash();
        }
    }
    void Crash()
    {
        GetComponent<Animator>().SetTrigger("Dead");
        hit = true;
    }
    void DestroyThis()
    {
        Destroy(gameObject);
    }
}
