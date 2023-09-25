using System.Collections;
using UnityEngine;

public class Square : MonoBehaviour
{
    public float delayTime;
    public GameObject arrow;
    void OnEnable()
    {
        StartCoroutine("ArrowAttack");
    }
    IEnumerator ArrowAttack()
    {
        yield return new WaitForSeconds(delayTime);
        for(int i = 0; i < 3; i++)
        {
            Instantiate(arrow, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 20.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, -90.0f));
            yield return new WaitForSeconds(0.1f);
        }
    }
}
