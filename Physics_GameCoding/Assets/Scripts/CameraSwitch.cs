using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    public Camera cam3;

    private Camera currentCam;

    void Start()
    {
        currentCam = cam1;

        cam1.enabled = true;
        cam2.enabled = false;
        cam3.enabled = false;
    }

    void Update()
    {
        // Press 2 switch to cam2 
        if (Input.GetKeyDown(KeyCode.Alpha2) && currentCam == cam1)
        {
            SwitchCamera(cam2);
        }

        // Press 1 switch to cam1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCamera(cam1);
        }

        // Press 3
        if (Input.GetKeyDown(KeyCode.Alpha3) && currentCam == cam1)
        {
            if (currentCam == cam1)
            {
                SwitchCamera(cam3);
            }
            
        }
    }

    void SwitchCamera(Camera newCam)
    {
        currentCam.enabled = false;
        newCam.enabled = true;
        currentCam = newCam;
    }
}