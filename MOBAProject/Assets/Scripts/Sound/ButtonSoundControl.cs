using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundControl : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    [SerializeField] AudioSource hoverSound;
    [SerializeField] AudioSource clickSound;

    public void PlayButtonClick()
    {
        hoverSound.PlayOneShot(hoverSound.clip);
    }

    public void PlayButtonHover()
    {
        clickSound.PlayOneShot(clickSound.clip);
    }
}
