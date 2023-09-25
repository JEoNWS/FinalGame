using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneInputManager : MonoBehaviour
{
    public Animator titleAnim;
    private void Update()
    {
        if (titleAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            if (Input.anyKeyDown)
            {
                titleAnim.SetBool("Clicked", true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                titleAnim.SetBool("Clicked", false);
            }
        }
    }
    public void SceneManager_Start()
    {
        SceneChangeManager.GetInstance().Go_To_Stage00();
    }
    public void SceneManager_Cradit()
    {
        SceneChangeManager.GetInstance().Go_To_Stage00();
    }
    public void SceneManager_Quit()
    {
        SceneChangeManager.GetInstance().Quit();
    }
}
