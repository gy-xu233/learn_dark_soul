using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float cameraDampValue;
    public Image lockDot;
    public bool lockState;

    private GameObject model;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float horizonX = 0;
    private GameObject Mcamera;
    private Vector3 cameraDampVelocity;
    [SerializeField]
    private GameObject lockTarget;

    // Start is called before the first frame update
    private void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        model = playerHandle.GetComponent<ActorController>().model;
        Mcamera = Camera.main.gameObject;
        lockDot.enabled = false;
        lockState = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!lockState)
        {
            Vector3 modelAngular = model.transform.eulerAngles;
            playerHandle.transform.Rotate(Vector3.up, pi.JRight * horizontalSpeed * Time.fixedDeltaTime);
            horizonX -= pi.JUp * verticalSpeed * Time.fixedDeltaTime;
            horizonX = Mathf.Clamp(horizonX, -20, 50);
            cameraHandle.transform.localEulerAngles = new Vector3(horizonX, 0, 0);
            model.transform.eulerAngles = modelAngular;
        }
        else
        {
            lockDot.transform.position = Camera.main.WorldToScreenPoint(lockTarget.transform.position);
            Vector3 forwardVector = lockTarget.transform.position - playerHandle.transform.position;
            forwardVector.y = 0;
            playerHandle.transform.forward = forwardVector.normalized;
        }

        //Mcamera.transform.eulerAngles = transform.eulerAngles;
        Mcamera.transform.position = Vector3.SmoothDamp(Mcamera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        Mcamera.transform.LookAt(cameraHandle.transform);
    }

    public void LockUnLock()
    {
        Vector3 boxCenter = playerHandle.transform.position + new Vector3(0, 1, 0) + playerHandle.transform.forward * 5;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), playerHandle.transform.rotation, LayerMask.GetMask("Enemy"));
        
        foreach (var item in cols)
        {
            if (item .gameObject== lockTarget)
            {
                lockTarget = null;
                lockDot.enabled = false;
                lockState = false;
            }
            else
            {
                lockTarget = item.gameObject;
                lockDot.enabled = true;
                lockState = true;
            }
            break;
        }
        if (cols.Length == 0)
        {
            lockTarget = null;
            lockDot.enabled = false;
            lockState = false;
        }
    }
}
