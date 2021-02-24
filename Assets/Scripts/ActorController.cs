﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    [Space(10)]
    [Header("friction signal")]
    public PhysicMaterial materialOne;
    public PhysicMaterial materialZero;
    public GameObject model;

    private CapsuleCollider col;
    private float movinSpeed = 2.0f;
    private float jumpHeight = 3.8f;
    private bool OnGround;
    private float fall;
    private float rollHeight = 3.0f;

    private bool LockPlanar;
    private bool canAttack;
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
        fall = 0;
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
        anim.SetFloat("forward", Mathf.Lerp(anim.GetFloat("forward"), PInput.Dmag,0.05f));
        anim.SetFloat("fall", (OnGround ? 0 :(Time.time - fall)));
        anim.SetBool("defence", PInput.defence);
        if(PInput.attack&&checkState("ground")&&anim.GetBool("onGround")&&canAttack)
        {
            anim.SetTrigger("attack");
        }
        if(PInput.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        if(PInput.Dmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward,PInput.Dvec,0.12f);
        }
        if (!LockPlanar)
        {
            planarVec = PInput.Dmag * model.transform.forward * movinSpeed;
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
        if (OnGround) fall = Time.time;
        OnGround = false;
        anim.SetBool("onGround", false);
    }

    public void JumpOnEnter()
    {
        PInput.inputEnable = false;
        LockPlanar = true;
        extraVec = new Vector3(0, jumpHeight, 0);
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