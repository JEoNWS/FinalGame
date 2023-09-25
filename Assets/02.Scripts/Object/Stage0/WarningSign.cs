using UnityEngine;

public class WarningSign : MonoBehaviour
{
    SpriteRenderer sr;
    bool colorToRed;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (colorToRed)
            sr.color += new Color(0.0f, 1.0f, 1.0f, 0.0f) * Time.deltaTime * 2.0f;
        else
            sr.color -= new Color(0.0f, 1.0f, 1.0f, 0.0f) * Time.deltaTime * 2.0f;

        if (sr.color.b <= 0.0f)
            colorToRed = true;
        else if(sr.color.b >= 1.0f)
            colorToRed = false;
    }
}
