using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayerer : MonoBehaviour
{
    Animator animator;
    public float delay=0;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DelayPlay());

    }

    IEnumerator DelayPlay() 
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger("triggerPlay") ;
    
    }
}
