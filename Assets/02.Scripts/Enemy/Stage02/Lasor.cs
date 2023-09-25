using UnityEngine;

public class Lasor : MonoBehaviour
{
    Player player;
    void DestroyThis()
    {
        transform.parent.GetComponent<RobotBoss>().LasorAttackEnd();
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
    }

    void Attack()
    {
        if (player)
        {
            player.playerCanHit = true;
            player.Hit();
        }
    }

}
