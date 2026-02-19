using UnityEngine;

public class UIManager : MonoBehaviour
{
    // geometric shapes
    public const int CUBE = 0;
    public const int SPHERE = 1;
    public const int CAPSULE = 2;

    // textures
    public const int material1 = 0;
    public const int material2 = 1;
    public const int material3 = 2;

    // delegates
    public delegate void ChangeGeometry(int value);
    public static event ChangeGeometry OnChangeGeometry;

    public delegate void ChangeTexture(int value);
    public static event ChangeTexture OnChangeTexture;

    public void geoButtonClick(int i)
    {
        OnChangeGeometry?.Invoke(i);
    }

    public void textureButtonClick(int i)
    {
        OnChangeTexture?.Invoke(i);
    }
}
