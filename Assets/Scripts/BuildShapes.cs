using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMouseInput : MonoBehaviour
{

    public int direction = 1;
    public int geoIndex = 0;

    // for textures
    public Material cubeMaterial;
    public Material sphereMaterial;
    public Material capsuleMaterial;

    private void OnEnable()
    {
        UIManager.OnChangeGeometry += UIManagerOnChangeGeometry;
    }

    private void OnDisable()
    {
        UIManager.OnChangeGeometry -= UIManagerOnChangeGeometry;
    }

    // ui buttons to change shapes
    private void UIManagerOnChangeGeometry(int value)
    {
        geoIndex = value;
        switch (value)
        {
            case 0:
                Debug.Log("Cube Selected");
                break;
            case 1:
                Debug.Log("Sphere Selected");
                break;
            case 2:
                Debug.Log("Capsule Selected");
                break;
        }
    }

    private GameObject myGO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"Hello from the Start function!");
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))  // check if left button is pressed
        {
            // take mouse position, convert from screen space to world space, do a raycast, store output of raycast into 
            // hitInfo object ...

            #region Screen To World
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                switch (geoIndex)
                {
                    case 0:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        myGO.GetComponent<Renderer>().material = cubeMaterial;
                        break;
                    case 1:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        myGO.GetComponent<Renderer>().material = sphereMaterial;
                        break;
                    case 2:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        myGO.GetComponent<Renderer>().material = capsuleMaterial;
                        break;
                }

                if (hitInfo.transform.tag.Equals("Ground"))
                {
                    myGO.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + (0.5f), hitInfo.point.z);
                }

                else
                {
                    if (hitInfo.normal == new Vector3(0, 0, 1)) // z+
                    {
                        myGO.transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.point.z + (0.5f));
                    }
                    if (hitInfo.normal == new Vector3(1, 0, 0)) // x+
                    {
                        myGO.transform.position = new Vector3(hitInfo.point.x + (0.5f), hitInfo.transform.position.y, hitInfo.transform.position.z);
                    }
                    if (hitInfo.normal == new Vector3(0, 1, 0)) // y+
                    {
                        myGO.transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.point.y + (0.5f), hitInfo.transform.position.z);
                    }
                    if (hitInfo.normal == new Vector3(0, 0, -1)) // z-
                    {
                        myGO.transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.transform.position.y, hitInfo.point.z - (0.5f));
                    }
                    if (hitInfo.normal == new Vector3(-1, 0, 0)) // x-
                    {
                        myGO.transform.position = new Vector3(hitInfo.point.x - (0.5f), hitInfo.transform.position.y, hitInfo.transform.position.z);
                    }
                    if (hitInfo.normal == new Vector3(0, -1, 0)) // y-
                    {
                        myGO.transform.position = new Vector3(hitInfo.transform.position.x, hitInfo.point.y - (0.5f), hitInfo.transform.position.z);
                    }
                }

            }
            else
            {
                Debug.Log("No hit");
            }
            #endregion
        }

    }

}