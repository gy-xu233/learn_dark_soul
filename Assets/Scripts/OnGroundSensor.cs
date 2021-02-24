using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capsule;

    private Vector3 vec1;
    private Vector3 vec2;
    private float radius;
    private float offset = 0.2f;

    private void Awake()
    {
        radius = capsule.radius - 0.1f;
    }

    private void FixedUpdate()
    {
        vec1 = transform.position + transform.up * (radius - offset);
        vec2 = transform.position + transform.up * (capsule.height - offset) - transform.up * radius;
        Collider[] outputCol = Physics.OverlapCapsule(vec1, vec2, radius, LayerMask.GetMask("Ground"));
        if(outputCol.Length != 0)
        {
            //foreach (Collider Col in outputCol)
            //{
            //    Debug.Log(Col.name);
            //}
            gameObject.SendMessage("IsOnGround");
        }
        else
        {
            gameObject.SendMessage("NotOnGround");
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
