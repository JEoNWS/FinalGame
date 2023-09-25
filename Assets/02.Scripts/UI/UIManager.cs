using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    [SerializeField]
    GameObject fadeOut;
    public Sprite[] player_HP;
    public Sprite[] player_Stamina;
    public Sprite[] bujuk;
    public Sprite[] bujukGauge;
    public Image Showing_Player_HP;
    public Image Showing_Player_Stamina;
    public Image Bujuk;
    public Image BujukGauge;
    public GameObject pauseObject;
    void Start()
    {
        if (instance != null)
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }
    public static UIManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<UIManager>();

            if (instance == null)
            {
                GameObject container = new GameObject("UIManager");
                instance = container.AddComponent<UIManager>();
                container.AddComponent<Camera>();
            }
        }
        return instance;
    }
    void Update()
    {
        
    }

    public void ResetPlayerHP()
    {
        Showing_Player_HP.sprite = player_HP[player_HP.Length - 1];
    }

    public void ChangePlayerHP(int currentPlayerHP)
    {
        Showing_Player_HP.sprite = player_HP[currentPlayerHP];
    }

    public void ChangeBujuk(int currentAttackCount)
    {
        Bujuk.sprite = bujuk[currentAttackCount];
        BujukGauge.sprite = bujukGauge[currentAttackCount];
    }

    public void Pause_Resume()
    {
        pauseObject.SetActive(false);
        InputManager.GetInstance().Resume();
        Time.timeScale = 1;
    }
    public void Pause_Quit()
    {
        pauseObject.SetActive(false);
        Time.timeScale = 1;
        InputManager.GetInstance().Resume();
        SceneChangeManager.GetInstance().Go_To_Main();
    }
    public void ChangeStamina()
    {
        Showing_Player_Stamina.sprite = player_Stamina[Player.GetInstance().dodgeCount];
    }

    public void FadeOut()
    {
        fadeOut.SetActive(true);
    }
}
