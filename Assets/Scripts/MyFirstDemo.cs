using UnityEngine;

public class MyFirstDemo : MonoBehaviour
{

    public int direction = 1;

    public int geoIndex;

    private void OnEnable()
    {
        UIManager.OnChangeGeometry += UIManagerOnChangeGeometry;
    }

    private void OnDisable()
    {
        UIManager.OnChangeGeometry -= UIManagerOnChangeGeometry;
    }

    private void UIManagerOnChangeGeometry(int value)
    {
        Debug.Log($"OnChangeGeometry (value)");
        geoIndex = value;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"Hello from the Start function!");
    }

    private GameObject myGO;

    void Update()
    {
        RaycastHit hitInfo;

        if (Input.GetMouseButton(0))
        {
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                switch (geoIndex)
                {
                    case 0:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        break;
                    case 1:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        break;
                    case 2:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        break;
                }

                if(hitInfo.transform.tag.Equals("Ground"))
                {
                    myGO.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y * 0.5f, hitInfo.point.z);
                }

                else
                {
                    myGO.transform.position = hitInfo.transform.position + hitInfo.normal;
                }
 
            }
        }

    }

    void FixedUpdate()
    {
        // Debug.Log($"Hello from the FixedUpdate function!");

    }
}
