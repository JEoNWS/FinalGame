using UnityEngine;

public class ArrowRain : MonoBehaviour
{
    void OnEnable()
    {
        if(Random.Range(0, 2) == 0)
        {
            transform.position = new Vector3(148.0f, 0.0f, 0.0f);
        }
        else
        {
            transform.position = new Vector3(152.0f, 0.0f, 0.0f);
        }
        Invoke("SetActiveFalse", 3.0f);
    }
    void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
