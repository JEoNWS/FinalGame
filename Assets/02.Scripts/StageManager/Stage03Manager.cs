using UnityEngine;

public class Stage03Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private GameObject[] enemies;
    public static int deadEnemyCount;
    [SerializeField]
    float xMove;
    [SerializeField]
    int doOnce;
    void Start()
    {
        CameraManager.GetInstance().cantMove = true;
        deadEnemyCount = 0;
        doOnce = 0;
    }

    void Update()
    {
        switch(deadEnemyCount)
        {
            case 1:
                if (doOnce == 0)
                {
                    wall.transform.GetChild(0).transform.position += Vector3.right * xMove;
                    CameraManager.GetInstance().ResetCamera();
                    CameraManager.GetInstance().cantMove = false;
                    doOnce++;
                }
                break;
            case 2:
                if (doOnce == 2)
                {
                    wall.transform.GetChild(0).transform.position += Vector3.right * xMove;
                    CameraManager.GetInstance().ResetCamera();
                    CameraManager.GetInstance().cantMove = false;
                    doOnce++;
                }
                break;
            case 3:
                if (doOnce == 4)
                {
                    wall.transform.GetChild(0).transform.position += Vector3.right * xMove;
                    CameraManager.GetInstance().cantMove = false;
                    CameraManager.GetInstance().ResetCamera();
                    doOnce++;
                }
                break;
            case 4:
                if (doOnce == 6)
                {
                    wall.transform.GetChild(0).transform.position = new Vector3(150.75f, 0.0f, 0.0f);
                    CameraManager.GetInstance().cantMove = false;
                    CameraManager.GetInstance().ResetCamera();
                    doOnce++;
                }
                break;
            default:
                return;
        }
        switch(doOnce)
        {
            case 1:
                if (CameraManager.GetInstance().transform.position.x >= (16.0f) + (xMove) * (doOnce + 1) / 2)
                {
                    CameraManager.GetInstance().cantMove = true;
                    CameraManager.GetInstance().transform.position = new Vector3((16.0f) + (xMove) * (doOnce + 1) / 2, 4.0f, -10.0f);
                    wall.transform.GetChild(1).transform.position += Vector3.right * xMove;
                    enemies[doOnce / 2].SetActive(true); // 0
                    doOnce++;
                }
                break;
            case 3:
                if (CameraManager.GetInstance().transform.position.x >= (16.0f) + (xMove) * (doOnce + 1) / 2)
                {
                    CameraManager.GetInstance().cantMove = true;
                    CameraManager.GetInstance().transform.position = new Vector3((16.0f) + (xMove) * (doOnce + 1) / 2, 4.0f, -10.0f);
                    wall.transform.GetChild(1).transform.position += Vector3.right * xMove;
                    enemies[doOnce / 2].SetActive(true);
                    doOnce++;
                }
                break;
            case 5:
                if (CameraManager.GetInstance().transform.position.x >= (16.0f) + (xMove) * (doOnce + 1) / 2)
                {
                    CameraManager.GetInstance().cantMove = true;
                    CameraManager.GetInstance().transform.position = new Vector3((16.0f) + (xMove) * (doOnce + 1) / 2, 4.0f, -10.0f);
                    wall.transform.GetChild(1).transform.position += Vector3.right * xMove;
                    enemies[doOnce / 2].SetActive(true);
                    doOnce++;
                }
                break;
            case 7:
                if (CameraManager.GetInstance().transform.position.x >= (16.0f) + (xMove) * (doOnce + 1) / 2)
                {
                    CameraManager.GetInstance().cantMove = true;
                    CameraManager.GetInstance().transform.position = new Vector3(135.5f, 4.0f, -10.0f);
                    wall.transform.GetChild(1).transform.position = new Vector3(120.25f, 10.0f, 0.0f);
                    enemies[doOnce / 2].SetActive(true);
                    doOnce++;
                }
                break;
        }
    }
}
