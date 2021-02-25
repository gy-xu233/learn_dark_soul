using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [Space(10)]
    [Header("friction signal")]
    public PhysicMaterial materialOne;
    public PhysicMaterial materialZero;

    public GameObject model;
    public CameraController camCon;

    private CapsuleCollider col;
    private float movinSpeed = 2.0f;
    private float jumpHeight = 3.8f;
    private bool OnGround;
    private float rollHeight = 3.0f;

    private bool LockPlanar;
    private bool trackDirection;
    private bool canAttack;
    private bool canRoll;
    private Animator anim;
    private PlayerInput PInput;
    private Vector3 planarVec;
    private Vector3 extraVec;
    private Rigidbody rigid_body;
    private float targetLerp;
    private Vector3 deltaPos;

    private void Awake()
    {
        anim = model.GetComponent<Animator>();
        PInput = GetComponent<PlayerInput>();
        rigid_body = GetComponent<Rigidbody>();
        LockPlanar = false;
        OnGround = true;
        col = GetComponent<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("defence", PInput.defence);
        if ((PInput.roll && canRoll) || rigid_body.velocity.magnitude > 7f)
        {
            anim.SetTrigger("roll");
            canAttack = false;

        }

        if (PInput.jump && canRoll)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }

        if (PInput.attack&&checkState("ground")&&anim.GetBool("onGround")&&canAttack)
        {
            anim.SetTrigger("attack");
            canRoll = false;
        }

        if (PInput.mLock)
        {
            camCon.LockUnLock();
        }
        if(!camCon.lockState)
        {
            anim.SetFloat("forward", Mathf.Lerp(anim.GetFloat("forward"), PInput.Dmag, 0.05f));
            if (PInput.Dmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, PInput.Dvec, 0.12f);
            }
            if (!LockPlanar)
            {
                planarVec = PInput.Dmag * model.transform.forward * movinSpeed;
            }
        }
        else
        {
            Vector3 localDvec = PInput.Dmag * transform.InverseTransformVector(PInput.Dvec);
            anim.SetFloat("forward", Mathf.Lerp(anim.GetFloat("forward"), localDvec.z, 0.05f));
            anim.SetFloat("right", Mathf.Lerp(anim.GetFloat("right"), localDvec.x, 0.05f));
            
            if(!trackDirection)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }
            if (!LockPlanar)
            {
                planarVec = PInput.Dvec.normalized * PInput.Dmag * movinSpeed;
            }
        }
    }

    private void FixedUpdate()
    {
        //rigid_body.position += planarVec * Time.fixedDeltaTime * movinSpeed;
        rigid_body.position += deltaPos;
        rigid_body.velocity = new Vector3(planarVec.x, rigid_body.velocity.y, planarVec.z) + extraVec;
        extraVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }


    private bool checkState(string stateName, string layerName = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerName)).IsName(stateName);
    }

    /// <summary>
    /// message
    /// </summary>

    public void IsOnGround()
    {
        OnGround = true;
        anim.SetBool("onGround", true);
    }

    public void NotOnGround()
    {
        OnGround = false;
        anim.SetBool("onGround", false);
    }

    public void JumpOnEnter()
    {
        PInput.inputEnable = false;
        LockPlanar = true;
        extraVec = new Vector3(0, jumpHeight, 0);
        trackDirection = true;
    }
    public void FallOnEnter()
    {
        PInput.inputEnable = false;
        LockPlanar = true;
    }
    public void OnGroundEnter()
    {
        PInput.inputEnable = true;
        LockPlanar = false;
        trackDirection = false;
        canAttack = true;
        col.material = materialOne;
    }

    public void OnGroundExit()
    {
        col.material = materialZero;
    }

    public void RollOnEnter()
    {
        extraVec = new Vector3(0, rollHeight, 0);
        PInput.inputEnable = false;
        LockPlanar = true;
        trackDirection = true;
    }
    public void JabOnEnter()
    {
        PInput.inputEnable = false;
        LockPlanar = true;
    }
    public void JabOnUpdate()
    {
        extraVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void Attack1OnUpdate()
    {
        extraVec = model.transform.forward * anim.GetFloat("attack1Velocity");
        anim.SetLayerWeight(anim.GetLayerIndex("attack"),Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), targetLerp, 0.1f));
    }

    public void OnAttack1Enter()
    {
        PInput.inputEnable = false;
        targetLerp = 1.0f;
    }
    public void Attack1IdleOnUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("attack")), targetLerp, 0.1f));
    }
    public void OnAttackIdleEnter()
    {
        PInput.inputEnable = true;
        LockPlanar = false;
        canRoll = true;
        targetLerp = 0f;
    }

    public void OnRMUpdate(object _deltaPos)
    {
        if(checkState("attack1hC","attack"))
        {
            deltaPos += (Vector3)_deltaPos;
        }
    }
}
