using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour
{
    private Animator anim;
    public Vector3 fixVector;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        //if (!anim.GetBool("defence"))
        //{
        //    Transform leftarm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        //    leftarm.localEulerAngles += fixVector;
        //    anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftarm.localEulerAngles));
        //}
    }
}
