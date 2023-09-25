using UnityEngine;

public class Stage02Manager : MonoBehaviour
{
    [SerializeField]
    GameObject Boss;
    [SerializeField]
    string nextSceneManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player.GetInstance().Heal();
            Boss.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
        }   
    }
    public void Clear()
    {
        SceneChangeManager.GetInstance().LoadScene(nextSceneManager);
    }
}
