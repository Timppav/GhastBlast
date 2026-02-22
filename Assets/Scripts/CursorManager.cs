using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D crosshairCursor;
    [SerializeField] private Texture2D menuCursor;

    void Start()
    {
        SetCrosshairCursor();
    }

    public void SetCrosshairCursor()
    {
        SetCursor(crosshairCursor);
    }

    public void SetMenuCursor()
    {
        SetCursor(menuCursor);
    }

    void SetCursor(Texture2D texture)
    {
        Vector2 hotspot = new Vector2(texture.width / 2, texture.height / 2);
        Cursor.SetCursor(texture, hotspot, CursorMode.Auto);
    }
}
