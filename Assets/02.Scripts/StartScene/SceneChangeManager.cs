using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    private static SceneChangeManager sceneManager;
    public bool canChangeSceneAsync;
    private void Start()
    {
        canChangeSceneAsync = false;
        Application.targetFrameRate = 60;
        if (sceneManager != null)
        {
            if (sceneManager != this)
                Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public static SceneChangeManager GetInstance()
    {
        if (sceneManager == null)
        {
            sceneManager = FindObjectOfType<SceneChangeManager>();

            if (sceneManager == null)
            {
                GameObject container = new GameObject("SceneManager");
                sceneManager = container.AddComponent<SceneChangeManager>();
            }
        }
        return sceneManager;
    }
    public void Go_To_Stage00()
    {
        SceneManager.LoadScene("CutScene");
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneA(string sceneName)
    {
        StartCoroutine("AsyncSceneLoad", sceneName);

    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Go_To_Main()
    {
        Destroy(FindObjectOfType<SceneChangeManager>().gameObject);
        Destroy(FindObjectOfType<BGM>().gameObject);
        SceneManager.LoadScene(0);
    }

    IEnumerator AsyncSceneLoad(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        while (true)
        {
            if (canChangeSceneAsync)
            {
                async.allowSceneActivation = true;
                canChangeSceneAsync = false;
                break;
            }
            yield return null;
        }
    }
}
