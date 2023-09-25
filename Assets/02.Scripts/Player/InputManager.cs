using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager inputManager;
    public bool cantInput;
    private bool pause;
    private bool dialogue;
    private void Start()
    {
        if (inputManager != null)
        {
            if (inputManager != this)
                Destroy(gameObject);
        }
        pause = false;
    }

    public static InputManager GetInstance()
    {
        if (inputManager == null)
        {
            inputManager = FindObjectOfType<InputManager>();

            if (inputManager == null)
            {
                GameObject container = new GameObject("InputManager");
                inputManager = container.AddComponent<InputManager>();
            }
        }
        return inputManager;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pause)
            {
                UIManager.GetInstance().Pause_Resume();
                if (dialogue)
                {
                    pause = false;
                    return;
                }
                Resume();
                return;
            }
            Time.timeScale = 0;
            UIManager.GetInstance().pauseObject.SetActive(true);
            Pause();
        }
        if (cantInput)
            return;
        if (Player.GetInstance().dead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneChangeManager.GetInstance().ReloadScene();
            }
            return;
        }

        if (/*Input.GetMouseButtonDown(0)*/Input.GetButtonDown("Fire1"))
        {
            LeftClick();
        }
        else if(/*Input.GetMouseButtonDown(1)*/Input.GetButtonDown("Fire2"))
        {
            RightClick();
        }
        else if(Input.GetAxisRaw("Horizontal") != 0)
        {
            MoveKey(Input.GetAxisRaw("Horizontal"));
            // 달리면서 점프
            if (/*Input.GetKeyDown(KeyCode.W)*/Input.GetButtonDown("Jump"))
            {
                JumpKey();
            }
            else if (Input.GetAxis("Fire3") != 0)
            {
                DodgeKey();
            }
            else if (/*Input.GetKeyDown(KeyCode.S)*/Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
            {
                GroundFall();
            }
        }
        else if (Input.GetAxis("Fire3") != 0)
        {
            DodgeKey();
        }
        else if (/*Input.GetKeyDown(KeyCode.W)*/Input.GetButtonDown("Jump"))
        {
            JumpKey();
        }
        else if(/*Input.GetKeyDown(KeyCode.S)*/Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("Vertical") < 0)
        {
            GroundFall();
        }
        else
        {
            NoInput();
        }

    }

    private void LeftClick()
    {
        Player.GetInstance().AttackAnim();
    }

    private void RightClick()
    {
        Player.GetInstance().Power_Attack();
    }
    private void MoveKey(float a)
    {
        Player.GetInstance().Move(a);
    }
    private void JumpKey()
    {
        Player.GetInstance().Jump();
    }
    private void DodgeKey()
    {
        Player.GetInstance().Dodge();
    }
    private void NoInput()
    {
        Player.GetInstance().Stop();
    }
    public void GroundFall()
    {
        Player.GetInstance().GroundFall();
    }
    public void Resume()
    {
        cantInput = false;
        pause = false;
    }
    public void Pause()
    {
        cantInput = true;
        pause = true;
    }
    public void Dialogue()
    {
        dialogue = true;
        cantInput = true;
    }
    public void DialogueEnd()
    {
        dialogue = false;
        cantInput = false;
    }
}
