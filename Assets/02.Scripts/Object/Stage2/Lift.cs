using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField]
    Vector3 targetPos;
    [SerializeField]
    int standard;
    [SerializeField]
    float speed;
    [SerializeField]
    bool changeScene;
    [SerializeField]
    string nextSceneName;
    [SerializeField]
    Collider2D floorCollider;
    public static int deadEnemyCount;
    bool canMove;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(deadEnemyCount == standard)
        {
            if (canMove == false)
            {
                if (collision.CompareTag("Player"))
                {
                    canMove = true;
                    if (changeScene)
                    {
                        transform.GetChild(0).gameObject.SetActive(true);
                        UIManager.GetInstance().FadeOut();
                        floorCollider.enabled = false;
                    }
                }
            }
        }
    }
    void Update()
    {
        if(canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                Arrive();
        }
    }

    void Arrive()
    {
        deadEnemyCount = 0;
        if (changeScene)
        {
            SceneChangeManager.GetInstance().LoadScene(nextSceneName);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
