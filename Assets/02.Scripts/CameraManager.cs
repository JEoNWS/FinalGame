using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager cameraManager;
    float offset;
    public bool cantMove;
    private float cameraSize;
    [SerializeField]
    float xOffset;
    float yOffset;
    float shakePower;
    [SerializeField]
    int xMax;
    public int xMin;
    [SerializeField]
    int yMax;

    Vector3 currentPos;
    bool cameraShake;
    private void Awake()
    {
        cantMove = false;
    }
    private void Start()
    {
        if (cameraManager != null)
        {
            if (cameraManager != this)
                Destroy(gameObject);
        }
        offset = (16.0f / 9.0f * GetComponent<Camera>().orthographicSize);
        yOffset = GetComponent<Camera>().orthographicSize / 2;
    }
    public static CameraManager GetInstance()
    {
        if (cameraManager == null)
        {
            cameraManager = FindObjectOfType<CameraManager>();

            if (cameraManager == null)
            {
                GameObject container = new GameObject("MainCamera");
                cameraManager = container.AddComponent<CameraManager>();
                container.AddComponent<Camera>();
            }
        }
        return cameraManager;
    }
    void CameraShake(float power)
    {
        transform.position += new Vector3(Random.Range(-power, power), Random.Range(-power, power));
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3( Player.GetInstance().transform.position.x + xOffset, Player.GetInstance().transform.position.y + yOffset, -10.0f);
        targetPos = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5.0f);
        if (cantMove == false)
            transform.position = targetPos;
        if(cameraShake)
        {
            CameraShake(shakePower);
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin + offset, xMax - offset), Mathf.Clamp(transform.position.y, yOffset, yMax - yOffset), -10);
    }

    public void ShakeCamera(float time)
    {
        CancelInvoke("ResetCamera");
        if(!cameraShake)
            currentPos = transform.position;
        cameraShake = true;
        shakePower = time * 0.1f;
        Invoke("ResetCamera", time);
    }

    public void ResetCamera()
    {
        CancelInvoke("ResetCamera");
        //transform.position = currentPos;
        cameraShake = false;
    }

    public void ChangeCameraSize(float size)
    {
        cameraSize = size;
        StartCoroutine("Zoom");
    }

    IEnumerator Zoom()
    {
        while (GetComponent<Camera>().orthographicSize != cameraSize)
        {
            GetComponent<Camera>().orthographicSize = Mathf.MoveTowards(GetComponent<Camera>().orthographicSize, cameraSize, 0.1f);
            offset = (16.0f / 9.0f * GetComponent<Camera>().orthographicSize);
            yOffset = GetComponent<Camera>().orthographicSize / 2;
            yield return null;
        }
    }
}
