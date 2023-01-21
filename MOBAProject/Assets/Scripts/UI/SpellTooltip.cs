using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /* Written by Daniela
     * 
     */
    public GameObject tooltip;
    public Image spellIndicator;

    public void ShowToolTip()
    {
        tooltip.SetActive(true);
        if (spellIndicator != null)
            spellIndicator.enabled = true;
    }

    public void HideToolTip()
    {
        tooltip.SetActive(false);
        if (spellIndicator != null)
            spellIndicator.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideToolTip();
    }
}
