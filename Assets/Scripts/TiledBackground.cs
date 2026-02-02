using UnityEngine;

public class TiledBackground : MonoBehaviour
{
    [SerializeField] private Vector3 forcedScale = new Vector3 (3, 3, 0);
    
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Tiled;

        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;

        sr.size = new Vector2(width, height);
    }

    void Awake() { 
    
        transform.localScale = forcedScale;
    
    }
}
