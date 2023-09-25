using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    float currentPos;
    List<Enemy> enemies = new List<Enemy>();
    private void Start()
    {
        currentPos = transform.position.x;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemies.Add(collision.GetComponent<Enemy>());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        enemies.Remove(collision.GetComponent<Enemy>());
    }
    void Attack()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null) continue;
                enemies[i].Hit(currentPos - enemies[i].transform.position.x, 1);
            }
            CameraManager.GetInstance().ShakeCamera(0.5f);
        }
    }
    void AttackEarth()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null) continue;
                enemies[i].Hit(currentPos - enemies[i].transform.position.x, 200);
            }
            CameraManager.GetInstance().ShakeCamera(0.5f);
        }
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }
    public void Move()
    {
        transform.position += transform.right * 2.0f;
        Destroy(gameObject, 1.2f);
    }
}