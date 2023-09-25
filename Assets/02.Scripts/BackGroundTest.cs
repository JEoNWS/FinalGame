using UnityEngine;

public class BackGroundTest : MonoBehaviour
{
    public GameObject camera;
    public float endPoint;
    public float startPoint;
    public float cameraEndPoint;
    public float cameraStartPoint;
    private void Update()
    {
        transform.position = new Vector3(startPoint + ((camera.transform.position.x - cameraStartPoint) * (endPoint - startPoint) / (cameraEndPoint - cameraStartPoint)), transform.position.y, transform.position.z);
    }
}
