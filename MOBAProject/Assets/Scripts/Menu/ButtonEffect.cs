using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler
{
    /* Written by Daniela
     * 
     */
    [SerializeField]
    private Image buttonBackground;
    private Coroutine runningRoutine;
    private float maxAlpha = 0.15f;

    private void Awake()
    {
        buttonBackground.color = new Color(0, 0, 0, 0);
    }

    private void Start()
    {
        if (SoundControl.Instance != null)
            GetComponent<Button>().onClick.AddListener(SoundControl.Instance.buttonSoundControl.PlayButtonClick);
    }

    public void FadeInBackground()
    {
        if (runningRoutine != null)
        {
            StopCoroutine(runningRoutine);
        }
        runningRoutine = StartCoroutine(FadeInImage());
    }

    public void FadeOutBackground()
    {
        if (runningRoutine != null)
        {
            StopCoroutine(runningRoutine);
        }
        runningRoutine = StartCoroutine(FadeOutImage());
    }

    IEnumerator FadeOutImage()
    {
        for (float i = maxAlpha; i >= 0; i -= Time.deltaTime)
        {
            buttonBackground.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    IEnumerator FadeInImage()
    {
        for (float i = 0; i <= maxAlpha; i += Time.deltaTime)
        {
            buttonBackground.color = new Color(0, 0, 0, i);
            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // needs to be added through code since the inspector connection gets lost on scene reload
        if (SoundControl.Instance != null)
            SoundControl.Instance.buttonSoundControl.PlayButtonHover();
    }

}
