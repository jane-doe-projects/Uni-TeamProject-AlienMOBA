using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageIndicatorFade : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    public Color startColor, fadeToColor;
    public Vector3 initialOffset, finalOffset;
    public float fadeDuration;
    private float fadeStartTime;

    // Start is called before the first frame update
    void Start()
    {
        fadeStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float progress = (Time.time - fadeStartTime) / fadeDuration;
        if (progress <= 1)
        {
            transform.localPosition = Vector3.Lerp(initialOffset, finalOffset, progress);
            this.GetComponent<TextMeshPro>().color = Color.Lerp(startColor, fadeToColor, progress);
        } else
        {
            // destroy prefab (parent and its children (the text component)
            Destroy(gameObject.transform.parent.gameObject);
        }

    }
}
