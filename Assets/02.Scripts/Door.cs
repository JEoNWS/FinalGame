using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int sceneNum;
    public GameObject NPC;
    public bool sceneChange;
    public static int deadEnemyCount;
    [SerializeField]
    string nextScene;
    private void Start()
    {
        deadEnemyCount = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if (sceneChange)
            {
                GetComponent<Animator>().SetTrigger("SceneChange");
            }
            else
            {
                if(deadEnemyCount >= 3)
                    GetComponent<Animator>().SetTrigger("Open");
            }
        }
    }
    void SceneChange()
    {
        SceneChangeManager.GetInstance().LoadScene(nextScene);
    }
    void NPCAppear()
    {
        SceneChangeManager.GetInstance().LoadScene(nextScene);
        //NPC.SetActive(true);
        //InputManager.GetInstance().Dialogue();
    }
}
