using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorController : MonoBehaviour
{
    public void EndAnimator()
    {
        Animator animator = GetComponent<Animator>();
        animator.enabled = false;   
    }
}
