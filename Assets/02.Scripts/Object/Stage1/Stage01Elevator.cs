using UnityEngine;

public class Stage01Elevator : MonoBehaviour
{
    [SerializeField]
    bool isEnd;
    [SerializeField]
    string nextSceneName;
    [SerializeField]
    SpriteRenderer player;
    private void Start()
    {
        if (!isEnd)
        {
            GetComponent<Animator>().SetTrigger("Open");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isEnd)
        {
            if(collision.CompareTag("Player"))
            {
                GetComponent<Animator>().SetTrigger("Open");
            }
        }
    }
    void SpawnPlayer()
    {
        if (!isEnd)
            player.enabled = true;
        else
        {
            player.enabled = false;
            SceneChangeManager.GetInstance().LoadScene(nextSceneName);
        }
    }
}