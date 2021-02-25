using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("key setting")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyLock;
    public string keyRun;
    public string keyAttack;
    public string keyDefence;

    public string keyJUp;
    public string keyJDown;
    public string keyJLeft;
    public string keyJRight;

    [Header("output signal")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public float JUp;
    public float JRight;
    public bool roll;
    public bool run;
    public bool jump;
    public bool attack;
    public bool defence;
    public bool mLock;
    public bool move;


    [Header("other")]
    public Vector3 Dvec;
    public bool inputEnable;
    public bool mouseEnable;
    public float mouseSensitivityX;
    public float mouseSensitivityY;

    private float runSpeed = 2.0f;
    private float targetUp;
    private float targetRight;
    private float upVelocity;
    private float rightVelocity;
    
    private MyButton myButtonLock = new MyButton();
    private MyButton myButtonRun = new MyButton();
    private MyButton myButtonAttack = new MyButton();
    private MyButton myButtonDefence = new MyButton();

    private void Awake()
    {
        inputEnable = true;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputEnable)
        {
            myButtonRun.Tick(false);
            myButtonAttack.Tick(false);
            myButtonLock.Tick(false);
        }
        else
        {
            myButtonRun.Tick(Input.GetKey(keyRun));
            myButtonAttack.Tick(Input.GetKey(keyAttack));
            myButtonLock.Tick(Input.GetKey(keyLock));
        }
        if (mouseEnable)
        {
            JUp = Input.GetAxis("Mouse Y") * mouseSensitivityY;
            JRight = Input.GetAxis("Mouse X") * mouseSensitivityX;
        }
        else
        {
            JUp = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
            JRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);
        }
        targetUp = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetRight = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        defence = Input.GetKey(keyDefence);

        run = (myButtonRun.onPressing && !myButtonRun.isLagging) || myButtonRun.isExtending;
        jump = myButtonRun.isPressed && myButtonRun.isExtending;
        roll = myButtonRun.isReleased && myButtonRun.isLagging;
        attack = myButtonAttack.isPressed;
        mLock = myButtonLock.isPressed;

        if (!inputEnable)
        {
            targetRight = 0;
            targetUp = 0;
            jump = false;
            roll = false;
        }
        Dup = Mathf.SmoothDamp(Dup, targetUp, ref upVelocity,0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetRight, ref rightVelocity, 0.1f);

        Vector2 tempVector = Squard2Ciecle(new Vector2(Dright, Dup));
        float Dright2 = tempVector.x;
        float Dup2 = tempVector.y;
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2) * (run? runSpeed :1.0f);
        Dvec = Dup2 * transform.forward + Dright2 * transform.right;
    }

    private Vector2 Squard2Ciecle(Vector2 inputV)
    {
        Vector2 r = Vector2.zero;
        r.x = inputV.x * Mathf.Sqrt(1 - inputV.y * inputV.y / 2);
        r.y = inputV.y * Mathf.Sqrt(1 - inputV.x * inputV.x / 2);
        return r;
    }
}
