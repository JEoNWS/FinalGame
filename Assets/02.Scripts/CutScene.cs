using UnityEngine.Video;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    private void Start()
    {
        GetComponent<VideoPlayer>().loopPointReached += CutSceneEnd;
    }
    void CutSceneEnd(UnityEngine.Video.VideoPlayer vp)
    {
        SceneChangeManager.GetInstance().LoadScene("Stage00");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneChangeManager.GetInstance().LoadScene("Stage00");
    }
}
