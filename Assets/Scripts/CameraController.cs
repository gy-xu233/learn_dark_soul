using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float cameraDampValue;

    private GameObject model;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float horizonX = 0;
    private GameObject Mcamera;
    private Vector3 cameraDampVelocity;

    // Start is called before the first frame update
    private void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        model = playerHandle.GetComponent<ActorController>().model;
        Mcamera = Camera.main.gameObject;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 modelAngular = model.transform.eulerAngles;
        playerHandle.transform.Rotate(Vector3.up, pi.JRight * horizontalSpeed * Time.fixedDeltaTime);
        horizonX -= pi.JUp * verticalSpeed * Time.fixedDeltaTime;
        horizonX = Mathf.Clamp(horizonX, -20, 50);
        cameraHandle.transform.localEulerAngles = new Vector3(horizonX, 0, 0);
        model.transform.eulerAngles = modelAngular;

        //Mcamera.transform.eulerAngles = transform.eulerAngles;
        Mcamera.transform.position = Vector3.SmoothDamp(Mcamera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        Mcamera.transform.LookAt(cameraHandle.transform);
    }
}
