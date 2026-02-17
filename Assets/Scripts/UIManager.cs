using UnityEngine;

public class UIManager : MonoBehaviour
{
    // delegates
    public delegate void ChangeGeometry(int value);
    public static event ChangeGeometry OnChangeGeometry;

    public void myButtonClick(int i)
    {
        OnChangeGeometry?.Invoke(i);
    }
}
