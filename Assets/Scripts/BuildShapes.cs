using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyMouseInput : MonoBehaviour
{

    public int direction = 1;
    public int geoIndex = 0;
    public int textureIndex = 0;

    //  headers for textures
    public Material material1;
    public Material material2;
    public Material material3;

    private void OnEnable()
    {
        UIManager.OnChangeGeometry += UIManagerOnChangeGeometry;
        UIManager.OnChangeTexture += UIManagerOnChangeTexture;
    }

    private void OnDisable()
    {
        UIManager.OnChangeGeometry -= UIManagerOnChangeGeometry;
        UIManager.OnChangeTexture -= UIManagerOnChangeTexture;
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

    // ui buttons to change texture
    private void UIManagerOnChangeTexture(int value)
    {
        textureIndex = value;
        switch (value)
        {
            case 0:
                Debug.Log("Stone Blocks selected");
                break;
            case 1:
                Debug.Log("Stone Blocks 2 selected");
                break;
            case 2:
                Debug.Log("Metal Blocks selected");
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
        // prevent right clicking of ui buttons from rayscating and deleting stuff on plane
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))  // check if left button is pressed
        {
            // take mouse position, convert from screen space to world space, do a raycast, store output of raycast into 
            // hitInfo object ...

            #region Screen To World
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                //cube.GetComponent<BoxCollider>().isTrigger = true;
                //cube.GetComponent<Renderer>().material = blockMaterial;

                // create myGO object based on the button selected in the UI and places on plane
                switch (geoIndex)
                {
                    case 0:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //myGO.GetComponent<Renderer>().material = material1;
                        break;
                    case 1:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        //myGO.GetComponent<Renderer>().material = material2;
                        break;
                    case 2:
                        myGO = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        //myGO.GetComponent<Renderer>().material = material3;
                        break;
                }

                myGO.tag = "Block";

                // explosion script
                if (myGO.GetComponent<TriangleExplosion>() == null)
                {
                    myGO.AddComponent<TriangleExplosion>();
                }

                    // change textures based on button selected in the UI
                    switch (textureIndex)
                    {
                        case 0:
                            myGO.GetComponent<Renderer>().material = material1;
                            break;
                        case 1:
                            myGO.GetComponent<Renderer>().material = material2;
                            break;
                        case 2:
                            myGO.GetComponent<Renderer>().material = material3;
                            break;
                    }

                //cube.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.5f, hitInfo.point.z);
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

                Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.red, 2, false);
                Debug.Log(hitInfo.normal);

            }

            else
            {
                Debug.Log("No hit");
            }
            #endregion
        }

        else if (Input.GetMouseButtonUp(1)) // right click for removing and explosion
        {
            Debug.Log("Right click");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (!hit)
            {
                Debug.Log("No hit");
                return;
            }

            Debug.Log("Hit: " + hitInfo.transform.name + " Tag: " + hitInfo.transform.tag);

            // Don't remove the ground
            if (hitInfo.transform.CompareTag("Ground"))
            {
                return;
            }
            
            // only remove blocks you placed 
            if (!hitInfo.transform.CompareTag("Block"))
            {
                return;
            }

            TriangleExplosion exp = hitInfo.transform.GetComponent<TriangleExplosion>();
            if (exp != null)
            {
                StartCoroutine(exp.SplitMesh(true));
            }

            else
            {
                Destroy(hitInfo.transform.gameObject);
            }

        }

    }

}