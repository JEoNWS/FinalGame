using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.GetInstance().GetComponent<Collider2D>().isTrigger = false;
            GetComponent<Collider2D>().isTrigger = false;
        }
        else if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TurnAround();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player.GetInstance().GetComponent<Collider2D>().isTrigger = true;
            GetComponent<Collider2D>().isTrigger = true;

        }
    }
}
