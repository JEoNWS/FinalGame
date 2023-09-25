using UnityEngine;

public class StageConnectScene : MonoBehaviour
{
    [SerializeField]
    string nextSceneName;
    void Start()
    {
        SceneChangeManager.GetInstance().LoadSceneA(nextSceneName);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneChangeManager.GetInstance().canChangeSceneAsync = true;
        }
    }
}
