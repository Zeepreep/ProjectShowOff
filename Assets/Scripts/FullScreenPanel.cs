using UnityEngine;

public class FullScreenPanel : MonoBehaviour
{
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 1);
        rt.offsetMin = new Vector2(0, 0);
        rt.offsetMax = new Vector2(0, 0);
    }
}
