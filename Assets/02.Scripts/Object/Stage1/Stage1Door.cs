using UnityEngine;

public class Stage1Door : MonoBehaviour
{
    [SerializeField]
    GameObject bully;

    public void Spotted()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }
    void SpawnBully()
    {
        Instantiate(bully, transform.position, Quaternion.identity);
    }
}
