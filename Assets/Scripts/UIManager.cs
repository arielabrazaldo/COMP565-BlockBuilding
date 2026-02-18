using UnityEngine;

public class UIManager : MonoBehaviour
{
    public const int CUBE = 0;
    public const int SPHERE = 1;
    public const int CAPSULE = 2;

    // delegates
    public delegate void ChangeGeometry(int value);
    public static event ChangeGeometry OnChangeGeometry;

    public void myButtonClick(int i)
    {
        OnChangeGeometry?.Invoke(i);
    }
}
