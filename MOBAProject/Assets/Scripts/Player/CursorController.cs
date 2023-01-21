using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    public Texture2D normalCursorTexture;
    public Texture2D attackCursorTexture;
    public Texture2D friendlyCursorTexture;
    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    public bool isMainMenu;

    Ray ray;
    RaycastHit hit;

    int playerLayer = 8;
    int enemyLayer = 9;
    int hitLayer;


    private void Awake()
    {
        // change cursor visual
        SetDefaultCursor();
    }

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (!isMainMenu)
            CheckMouseOverUnitType();
    }

    private void SetDefaultCursor()
    {
        Cursor.SetCursor(normalCursorTexture, hotSpot, cursorMode);
    }

    private void SetAttackCursor()
    {
        // the cursor that is shown when hovering over enemy units
        Cursor.SetCursor(attackCursorTexture, hotSpot, cursorMode);
    }

    private void SetFriendlyCursor()
    {
        // the cursor that is shown when hovering over friendy units
        Cursor.SetCursor(friendlyCursorTexture, hotSpot, cursorMode);
    }

    private void CheckMouseOverUnitType()
    {
        // checks on which layer the object exists and changes the cursor accordingly, requires a collider on relevant objects for the raycast to work
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.collider)
            hitLayer = hit.collider.gameObject.layer;

        if (hitLayer == enemyLayer)
            SetAttackCursor();
        else if (hitLayer == playerLayer)
            SetFriendlyCursor();
        else
            SetDefaultCursor();
    }
}
