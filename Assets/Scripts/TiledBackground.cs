using UnityEngine;

public class TiledBackground : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Tiled;

        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;

        sr.size = new Vector2(width, height);
    }
}
