using UnityEngine;

public class Stage01Manager : MonoBehaviour
{
    [SerializeField]
    GameObject Unsim;
    [SerializeField]
    GameObject wall;
    [SerializeField]
    string nextSceneManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unsim.SetActive(true);
        wall.transform.position = Vector3.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        CameraManager.GetInstance().xMin = 0;
    }
    public void Clear()
    {
        SceneChangeManager.GetInstance().LoadScene(nextSceneManager);
    }
}
