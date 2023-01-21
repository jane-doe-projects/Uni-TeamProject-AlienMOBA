using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelIndicator : MonoBehaviour
{
    /* Written by Sebastian
     * 
     */
    private GameManager gameManager;
    private StateManager state;
    private TextMeshProUGUI textObject;
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        textObject = transform.GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        state = gameManager.state;
    }

    void Update()
    {
        textObject.text = state.PlayerLevel.ToString();
    }
}
