using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAlienAnimation : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    Animator alienAnimator;

    // Start is called before the first frame update
    void Start()
    {
        alienAnimator = GetComponent<Animator>();
        InvokeRepeating("PlayRandomAnimation", 2, 3);
    }

    private void PlayRandomAnimation()
    {
        alienAnimator.SetInteger("RandomAnimationIndex", Random.Range(0, 3));
    }
}
