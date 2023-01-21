using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsatePanelColor : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    private Image panelPulseImage;
    private float minAlpha = 0f;
    private float maxAlpha = 0.4f;
    private float fadeDuration = 1f;

    private void Awake()
    {
        panelPulseImage = GetComponent<Image>();
        maxAlpha = panelPulseImage.color.a;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("PulsateColor");
    }

    IEnumerator PulsateColor()
    {

        while (true)
        {
            panelPulseImage.CrossFadeAlpha(minAlpha, fadeDuration, false);
            yield return new WaitForSeconds(fadeDuration);
            panelPulseImage.CrossFadeAlpha(maxAlpha, fadeDuration, false);
            yield return new WaitForSeconds(fadeDuration);
        }
    }

}
