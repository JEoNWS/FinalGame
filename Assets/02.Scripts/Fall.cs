using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player.GetInstance().PlayerRollEnd();
            Player.GetInstance().Hit();
            collision.transform.position = new Vector3(3.0f, 1.5f, 0.0f);
        }
    }
}
